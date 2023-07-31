using JobManagementApi.Models.DTOS;

namespace JobManagementApi.Services
{
    public interface IJobService
    {
        Task<MultipleChoiseLists> GetMultipleChoiseLists();
        Task AddCV(int userId, CV cv);
        Task DeleteCV(int userId);
        Task EditCv(int userId, CV cv);
        Task AddSkills(int userId, SkillsCVTableModel skills);
        Task DeleteSkills(int userId, SkillIds skills);
    }
}
