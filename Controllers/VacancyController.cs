
using JobManagementApi.Models.DTOS;
using JobManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JobManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;
        private readonly IUserService _userService;
        public VacancyController(IVacancyService vacancyService, IUserService userService)
        {
            _userService = userService;
            _vacancyService = vacancyService;
        }
        [HttpPost("[action]")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddVacancy([FromHeader] string Authorization, Vacancy vacancy)
        {
            int managerId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _vacancyService.AddVacancy(managerId, vacancy);
            return Ok("Vacancy Succesfully Added");
        }

        [HttpPost("[action]/{jobId}")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> ApplyUserForJob(int jobId)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _vacancyService.ApplyUserForVacancy(userId, jobId);
            return Ok("User Succesfully Applied");
        }

        [HttpGet("[action]/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetTopCandidatesIds(int jobId)
        {
            return Ok(await _vacancyService.GetTopCandidates(jobId));
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetEmployeeVacancies()
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            return Ok(await _vacancyService.GetEmployeeVacancies(userId));
        }

        [HttpPut("[action]/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateVacancy(Vacancy vacancy, int jobId)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _vacancyService.UpdateVacancy(userId, jobId, vacancy);
            return Ok("Successfully Updated");
        }

        [HttpDelete("[action]/{jobId}/{skillId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteSkill(int jobId, int skillId)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            await _vacancyService.DeleteVacancy(userId, jobId, skillId);
            return Ok("Skill Successfully Deleted");
        }

        [HttpGet("[action]/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetCandidates(int jobId)
        {
            int managerId = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            return Ok(await _vacancyService.GetCandidates(jobId, managerId));
        }
    }
}
