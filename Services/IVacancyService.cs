using JobManagementApi.Models.DTOS;

namespace JobManagementApi.Services
{
    public interface IVacancyService
    {
        public Task AddVacancy(int managerId, Vacancy vacancy);
        public Task ApplyUserForVacancy(int userId, int jobId);
        public Task<List<int>> GetTopCandidates(int jobId);
        public Task<List<JobsTableModel>> GetEmployeeVacancies(int userId);
        public Task UpdateVacancy(int userId,int jobId, Vacancy vacancy);
        public Task DeleteVacancy(int userId, int jobId, int skillId);
        public Task<List<Candidate>> GetCandidates(int jobId, int managerId);
    }
}
