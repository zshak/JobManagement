namespace JobManagementApi.Models.Exceptions
{
    public class SkillAlreadyExistsException : BaseRequestException
    {
        public SkillAlreadyExistsException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
