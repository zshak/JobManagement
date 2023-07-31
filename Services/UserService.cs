using JobManagementApi.Extensions;
using JobManagementApi.JWT;
using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Models.Exceptions;
using JobManagementApi.Repos;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Security.Claims;
using System.Transactions;

namespace JobManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IVacancyRepo _vacancyRepo;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly Connector _connector;

        public UserService(IUserRepo userRepo, IJWTGenerator jWTGenerator, IOptions<Connector> connectionString, IVacancyRepo vacancyRepo)
        {
            _vacancyRepo = vacancyRepo;
            _userRepo = userRepo;
            _jwtGenerator = jWTGenerator;
            _connector = connectionString.Value;
        }

        public async Task<string> LoginUser(UserLoginModel user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                User DatabaseUser = await _userRepo.GetUserByLogin(user, connection);
                if (DatabaseUser == null) throw new UserNotFoundException("User Not Found", 404);
                return _jwtGenerator.GenerateToken(new[] { new Claim("id", DatabaseUser.Id.ToString()),
                                                            new Claim(ClaimTypes.Role, DatabaseUser.IsEmployer ? "Employer" : "Applicant")}
                                                    );
            }
        }

        public async Task<int> RegisterUser(UserRegisterModel user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    bool emailExists = await _userRepo.EmailExists(user.Email, connection, transaction);
                    if (emailExists) throw new EmailNotUniqueExpection("Email Already Exists", 400);
                    user.Password = user.Password.HashString();
                    int id = await _userRepo.RegisterUser(user, connection, transaction);
                    transaction.Commit();
                    return id;
                }
            }
        }
    }
}
