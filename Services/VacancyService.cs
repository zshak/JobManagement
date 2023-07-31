using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Models.Exceptions;
using JobManagementApi.Repos;
using Mapster;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;

namespace JobManagementApi.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepo _vacancyRepo;
        private readonly Connector _connector;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;

        public VacancyService(IUserRepo userRepo, IVacancyRepo vacancyRepo, IOptions<Connector> connectionString,
            IConfiguration configuration)
        {
            _vacancyRepo = vacancyRepo;
            _connector = connectionString.Value;
            _configuration = configuration;
            _userRepo = userRepo;

        }

        public async Task AddVacancy(int managerId, Vacancy vacancy)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    JobsVacancyModel model = vacancy.Adapt<JobsVacancyModel>();
                    int newJobId = await _vacancyRepo.AddVacancy(managerId, model, connection, transaction);
                    JobsReqSkillsModel skillsModel = vacancy.Adapt<JobsReqSkillsModel>();
                    await _vacancyRepo.AddVacancySkills(newJobId, skillsModel, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task ApplyUserForVacancy(int userId, int jobId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    if (await _vacancyRepo.GetUserByJobId(jobId, userId, connection) != -1)
                        throw new ApplicationAlreadyExistsException("User Already Applied", 400);
                    await _vacancyRepo.ApplyUserForVacancy(userId, jobId, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task<List<int>> GetTopCandidates(int jobId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    List<int> usersApplied = await _vacancyRepo.GetUsersAppliedForJob(jobId, connection, transaction);
                    List<VacancySkillModel> skillsReqForJob =
                        await _vacancyRepo.GetJobRequiredSkills(jobId, connection, transaction);
                    List<UserSkills> usersSkills = new List<UserSkills>();
                    foreach (var userID in usersApplied)
                    {
                        UserSkills user = new UserSkills();
                        user.UserId = userID;
                        user.Skills = await _vacancyRepo.GetUserSkills(userID, connection, transaction);
                        usersSkills.Add(user);
                    }

                    List<KeyValuePair<int, decimal>> scores = new List<KeyValuePair<int, decimal>>();
                    foreach (var user in usersSkills)
                    {
                        decimal? res = 0;
                        List<SkillTableModel> skills = user.Skills;
                        foreach (SkillTableModel skill in skills)
                        {
                            VacancySkillModel skillModel =
                                skillsReqForJob.FirstOrDefault(x => x.SkillId == skill.SkillId);
                            if (skillModel != null)
                            {
                                res += skillModel.Weight * (decimal)(skill.Experience) / skillModel.Experience;
                            }
                        }

                        scores.Add(new KeyValuePair<int, decimal>(user.UserId, (decimal)res));
                    }

                    List<int> topCandidates =
                        scores.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).ToList();
                    transaction.Commit();
                    return topCandidates.Take(_configuration.GetValue<int>("NumCandidates")).ToList();
                }
            }
        }

        public async Task<List<JobsTableModel>> GetEmployeeVacancies(int userId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    List<JobsTableModel> Vacancies =
                        await _vacancyRepo.GetEmployerVacancies(userId, connection, transaction);
                    foreach (var vacancy in Vacancies)
                    {
                        vacancy.Skills =
                            await _vacancyRepo.GetJobRequiredSkills(vacancy.JobId, connection, transaction);
                    }

                    transaction.Commit();
                    return Vacancies;
                }
            }

        }

        public async Task UpdateVacancy(int userId, int jobId, Vacancy vacancy)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    JobsVacancyModel jobsVacancy = vacancy.Adapt<JobsVacancyModel>();
                    int numRowsAffected =
                        await _vacancyRepo.UpdateVacancy(jobId, userId, jobsVacancy, connection, transaction);
                    if (numRowsAffected == 0) throw new VacancyNotFoundException("Vacancy Not Found", 400);
                    List<VacancySkillModel> skills = vacancy.Skills;
                    foreach (var skill in skills)
                    {
                        numRowsAffected =
                            await _vacancyRepo.UpdateVacancyReqSkills(jobId, skill, connection, transaction);
                        if (numRowsAffected == 0) throw new SkillNotFoundException("Skill Not Found", 400);
                    }

                    transaction.Commit();
                }
            }
        }

        public async Task DeleteVacancy(int userId, int jobId, int skillId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    if (!(await _vacancyRepo.GetEmployerVacancies(userId, connection, transaction)).Any(x =>
                            x.JobId == jobId))
                        throw new VacancyNotFoundException("Vacancy Not Found", 400);
                    int numRowsAffected =
                        await _vacancyRepo.DeleteVacancySkill(jobId, skillId, connection, transaction);
                    if (numRowsAffected == 0) throw new SkillNotFoundException("Skill Not Found", 400);
                    transaction.Commit();
                }
            }
        }

        public async Task<List<Candidate>> GetCandidates(int jobId, int managerID)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    List<JobsTableModel> jobs =
                        await _vacancyRepo.GetEmployerVacancies(managerID, connection, transaction);
                    if (!jobs.Any(x => x.JobId == jobId))
                        throw new VacancyNotFoundException("Manager Not Authorized to view Job: {jobId}", 401);
                    List<int> users = await GetTopCandidates(jobId);
                    List<Candidate> res = new List<Candidate>();
                    foreach (int userId in users)
                    {
                        Candidate candidate = await _userRepo.GetCandidatesById(userId, connection, transaction);
                         candidate.Skills = await _vacancyRepo.GetUserSkills(userId, connection, transaction);
                        res.Add(candidate);
                    }

                    transaction.Commit();
                    return res;
                }
            }
        }
    }
}
