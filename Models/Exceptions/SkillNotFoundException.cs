namespace JobManagementApi.Models.Exceptions
{
    public class SkillNotFoundException : BaseRequestException
    {
        public SkillNotFoundException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
