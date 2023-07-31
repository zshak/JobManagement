namespace JobManagementApi.Models.Exceptions
{
    public class EmailNotUniqueExpection : BaseRequestException
    {
        public EmailNotUniqueExpection(string message, int statusCode) : base(message, statusCode) { }
    }
}
