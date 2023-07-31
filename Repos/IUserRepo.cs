using JobManagementApi.Models.DTOS;
using Npgsql;

namespace JobManagementApi.Repos
{
    public interface IUserRepo
    {
        Task<int> RegisterUser(UserRegisterModel user, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<bool> EmailExists(string email, NpgsqlConnection connection, NpgsqlTransaction transaction);
        Task<User> GetUserByLogin(UserLoginModel user, NpgsqlConnection connection);
        Task<User> GetUserById(int id, NpgsqlConnection connection);
        Task<Candidate> GetCandidatesById(int userId,  NpgsqlConnection connection, NpgsqlTransaction transaction);
    }
}
