namespace JobManagementApi.Models.DTOS
{
    public class Candidate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Education { get; set; }
        public int Degree { get; set; }
        public int Profession { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SkillTableModel> Skills { get; set; }
    }
}
