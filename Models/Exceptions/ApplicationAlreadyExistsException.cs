namespace JobManagementApi.Models.Exceptions
{
    public class ApplicationAlreadyExistsException : BaseRequestException
    {
        public ApplicationAlreadyExistsException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
