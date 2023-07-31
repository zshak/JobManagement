using FluentValidation;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace JobManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMultipleChoice()
        {
            return Ok(await _jobService.GetMultipleChoiseLists());
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> AddCV([FromHeader] string Authorization, CV cv)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _jobService.AddCV(userId, cv);
            return Ok("CV Sucessfully Added");
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> DeleteCv([FromHeader] string Authorization)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _jobService.DeleteCV(userId);
            return Ok("CV Sucessfully Deleted");
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> UpdateCV([FromHeader] string Authorization, CV cv)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _jobService.EditCv(userId, cv);
            return Ok("CV Sucessfully Updated");
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> AddSkills([FromHeader] string Authorization, SkillsCVTableModel skills)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _jobService.AddSkills(userId, skills);
            return Ok("skills successfully added");
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> DeleteSkills([FromHeader] string Authorization, SkillIds skills)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _jobService.DeleteSkills(userId, skills);
            return Ok("Skills Successfully Deleted");
        }
    }
}
