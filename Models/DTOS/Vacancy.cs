namespace JobManagementApi.Models.DTOS
{
    public class Vacancy
    {
        public string? JobTitle { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<VacancySkillModel>? Skills { get; set; }
    }
}
