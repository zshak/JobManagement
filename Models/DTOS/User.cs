namespace JobManagementApi.Models.DTOS
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsEmployer { get; set; }
        public string? Company { get; set; }
    }
}
