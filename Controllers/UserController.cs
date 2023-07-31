using FluentValidation;
using FluentValidation.Results;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JobManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterModel> _userRegisterValidator;
        private readonly IValidator<UserLoginModel> _userLoginValidator;
        private readonly IUserService _userService;
        public UserController(IValidator<UserRegisterModel> userRegisterValidator, IUserService userService, IValidator<UserLoginModel> userLoginValidator)
        {
            _userRegisterValidator = userRegisterValidator;
            _userService = userService;
            _userLoginValidator = userLoginValidator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterUser(UserRegisterModel user)
        {
            ValidationResult res = _userRegisterValidator.Validate(user);
            if(!res.IsValid) 
            {
                return BadRequest(res);
            }

            await _userService.RegisterUser(user);
            return Ok("User Successfully Registered");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser(UserLoginModel user)
        {
            ValidationResult res = _userLoginValidator.Validate(user);
            if (!res.IsValid)
            {
                return BadRequest(res);
            }
            return Ok("Successfull Authentication\n" + await _userService.LoginUser(user));
        }

    }
}
