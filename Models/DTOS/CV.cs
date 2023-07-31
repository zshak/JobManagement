namespace JobManagementApi.Models.DTOS
{
    public class CV
    {
        public List<SkillTableModel>? Skills { get; set; }
        public int? Profession { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Degree { get; set; }
        public int? Education { get; set; }
    }
}
