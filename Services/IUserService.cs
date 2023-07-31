using JobManagementApi.Models.DTOS;

namespace JobManagementApi.Services
{
    public interface IUserService
    {
        Task<int> RegisterUser(UserRegisterModel user);
        Task<string> LoginUser(UserLoginModel user);
    }
}
