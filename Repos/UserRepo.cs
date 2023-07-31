using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using Microsoft.Extensions.Options;
using Npgsql;
using Dapper;
using JobManagementApi.Extensions;

namespace JobManagementApi.Repos
{
    public class UserRepo : IUserRepo
    {
        public async Task<bool> EmailExists(string email, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = "SELECT COUNT(*) FROM users WHERE email = @email";
            int numEmails = await connection.QuerySingleOrDefaultAsync<int>(query, new { email }, transaction);
            return numEmails > 0;
        }
        public async Task<int> RegisterUser(UserRegisterModel user, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = @"INSERT INTO users 
            (first_name, last_name, password, email, is_employer, company) 
            VALUES (@FirstName, @LastName, @Password, @Email, @IsEmployer, @Company) RETURNING id";
            return await connection.QuerySingleOrDefaultAsync<int>(query, 
                new {FirstName = user.FirstName, LastName = user.LastName, Password = user.Password,
                        Email = user.Email, IsEmployer = user.IsEmployer, @Company = user.Company}, transaction);
        }

        public async Task<User> GetUserByLogin(UserLoginModel user, NpgsqlConnection connection)
        {
            var query = @"SELECT id AS Id, first_name AS FirstName, 
                password AS Password, email AS Email, is_employer AS IsEmployer, company AS Company
            FROM users WHERE email = @Email and password = @Password";
            User res = await connection.QuerySingleOrDefaultAsync<User>(query, new { email = user.Email, Password = user.Password.HashString() });
            return res;
        }

        public async Task<User> GetUserById(int id, NpgsqlConnection connection)
        {
            var query = @"SELECT id AS Id, first_name AS FirstName, 
                password AS Password, email AS Email, is_employer AS IsEmployer, company AS Company
                FROM users WHERE id = @id";
            User res = await connection.QuerySingleOrDefaultAsync<User>(query, new {id});
            return res;
        }

        public async Task<Candidate> GetCandidatesById(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = @"SELECT u.first_name AS FirstName, u.last_name AS LastName, u.email AS Email, 
                    c.education AS Education, c.degree AS Degree, c.profession AS Profession, 
                    c.start_date AS StartDate, c.end_date AS EndDate
                    FROM users u
                    JOIN cvs c ON u.id = c.user_id
                    WHERE u.id = @userId";
            return await connection.QuerySingleOrDefaultAsync<Candidate>(query, new { userId }, transaction);
        }
    }
}
