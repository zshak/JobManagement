namespace JobManagementApi.Models.DTOS
{
    public class JobsTableModel
    {
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<VacancySkillModel>? Skills { get; set; }
    }
}
