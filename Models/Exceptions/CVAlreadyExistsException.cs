namespace JobManagementApi.Models.Exceptions
{
    public class CVAlreadyExistsException : BaseRequestException
    {
        public CVAlreadyExistsException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
