using JobManagementApi.Models.DTOS;
using System.Security.Claims;

namespace JobManagementApi.JWT
{
    public interface IJWTGenerator
    {
        string GenerateToken(Claim[] claims);
    }
}
