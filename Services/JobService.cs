using JobManagementApi.JWT;
using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Models.Exceptions;
using JobManagementApi.Repos;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Security.Claims;
using Mapster;

namespace JobManagementApi.Services
{
    public class JobService : IJobService
    {
        private readonly Connector _connector;
        private readonly IJobRepo _jobRepo;
        private readonly IUserRepo _userRepo;
        private readonly IVacancyRepo _vacancyRepo;

        public JobService(IUserRepo userRepo, IJobRepo jobRepo, IOptions<Connector> connectionString, IVacancyRepo vacancyRepo)
        {
            _userRepo = userRepo;
            _connector = connectionString.Value;
            _jobRepo = jobRepo;
            _vacancyRepo = vacancyRepo;
        }

        public async Task AddCV(int userId, CV cv)
        {
            using(NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using(NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    if (await _jobRepo.CVExists(userId, connection, transaction))
                        throw new CVAlreadyExistsException("CV Already Exists", 400);
                    CVTableModel cVTableModel = cv.Adapt<CVTableModel>();
                    await _jobRepo.AddCV(userId, cVTableModel, connection, transaction);
                    SkillsCVTableModel skilsCvTableModel = cv.Adapt<SkillsCVTableModel>();
                    await _jobRepo.AddSkills(userId, skilsCvTableModel, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task AddSkills(int userId, SkillsCVTableModel skills)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    List<SkillTableModel> userSkills = await _vacancyRepo.GetUserSkills(userId, connection, transaction);
                    if (!await _jobRepo.CVExists(userId, connection, transaction))
                        throw new CVDoesNotExistException("CV Does Not Exist", 400);
                    if (userSkills.Any(x => skills.Skills.Any(y => x.SkillId == y.SkillId)))
                        throw new SkillAlreadyExistsException("Skill already Exists", 400);
                    await _jobRepo.AddSkills(userId, skills, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task DeleteCV(int userId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    if (!await _jobRepo.CVExists(userId, connection, transaction))
                        throw new CVDoesNotExistException("CV Does Not Exist", 400);
                    await _jobRepo.DeleteCV(userId, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task DeleteSkills(int userId, SkillIds skills)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    await _jobRepo.DeleteSkill(userId, skills, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task EditCv(int userId, CV cv)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    if (!await _jobRepo.CVExists(userId, connection, transaction))
                        throw new CVDoesNotExistException("CV Does Not Exist", 400);
                    await _jobRepo.EditCV(userId, cv, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task<MultipleChoiseLists> GetMultipleChoiseLists()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                MultipleChoiseLists res = await _jobRepo.GetMultipleChoiseLists(connection);
                return res;
            }
        }
    }
}
