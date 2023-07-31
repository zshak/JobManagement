using FluentValidation;
using JobManagementApi.JWT;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Repos;
using JobManagementApi.Services;
using JobManagementApi.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobManagementApi.Extensions
{
    public static class ServiceExtentions
    {

        public static void AddJwt(this IServiceCollection services)
        {
            services.AddAuthentication("DS")//Default scheme
                .AddJwtBearer("DS"/* Schema name */, o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ajlsfhlkajhdsakljhfkljhasjklhgasjdghsklhljhl")),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserRegisterModel>, UserRegisterModelValidator>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepo, UserRepo>();
            services.AddSingleton<IJWTGenerator, JWTGenerator>();
            services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator> ();
            services.AddSingleton<IJobService, JobService>();
            services.AddSingleton<IJobRepo, JobRepo>();
            services.AddSingleton<IVacancyRepo, VacancyRepo>();
            services.AddSingleton<IVacancyService, VacancyService>();
            services.AddJwt();
        }
    }
}
