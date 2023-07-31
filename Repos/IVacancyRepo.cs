using JobManagementApi.Models.DTOS;
using Npgsql;

namespace JobManagementApi.Repos
{
    public interface IVacancyRepo
    {
        Task<int> AddVacancy(int managerId, JobsVacancyModel jobsVacancy, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task AddVacancySkills(int jobId, JobsReqSkillsModel jobsSkills, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<int> GetUserByJobId(int jobId, int userId, NpgsqlConnection connection);
        Task ApplyUserForVacancy(int userId, int jobId, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<List<int>> GetUsersAppliedForJob(int jobId, NpgsqlConnection connection, NpgsqlTransaction transaction);

        Task<List<VacancySkillModel>> GetJobRequiredSkills(int jobId, NpgsqlConnection connection,
            NpgsqlTransaction transaction);

        Task<List<SkillTableModel>> GetUserSkills(int userId, NpgsqlConnection connection,
            NpgsqlTransaction transaction);
        Task<List<JobsTableModel>> GetEmployerVacancies(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<int> UpdateVacancy(int jobId, int userId, JobsVacancyModel vacancy, NpgsqlConnection connection, NpgsqlTransaction transaction);

        Task<int> UpdateVacancyReqSkills(int jobId, VacancySkillModel skills, NpgsqlConnection connection,
            NpgsqlTransaction transaction);
        Task<int> DeleteVacancySkill(int jobId, int skillId, NpgsqlConnection connection, NpgsqlTransaction transaction);
    }
}
