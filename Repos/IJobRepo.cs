using JobManagementApi.Models.DTOS;
using Npgsql;

namespace JobManagementApi.Repos
{
    public interface IJobRepo
    {
        Task<MultipleChoiseLists> GetMultipleChoiseLists(NpgsqlConnection connection);
        Task<bool> CVExists(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task AddCV(int userId, CVTableModel cv, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task AddSkills(int userId, SkillsCVTableModel skills,  NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task DeleteCV(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task EditCV(int userId, CV cv, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task DeleteSkill(int userId, SkillIds skills, NpgsqlConnection connection, NpgsqlTransaction transaction);
    }
}
