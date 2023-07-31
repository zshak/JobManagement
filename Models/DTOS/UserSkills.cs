namespace JobManagementApi.Models.DTOS
{
    public class UserSkills
    {
        public int UserId { get; set; }
        public List<SkillTableModel> Skills { get; set; }
    }
}
