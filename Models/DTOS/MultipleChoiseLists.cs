namespace JobManagementApi.Models.DTOS
{
    public class MultipleChoiseLists
    {
        public List<Skill> Skills { get; set; }
        public List<Profession> Professions { get; set; }
        public List<Degree> Degrees { get; set; }
        public List<Education> Education { get; set; }
    }
}
