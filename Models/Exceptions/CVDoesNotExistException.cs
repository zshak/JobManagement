namespace JobManagementApi.Models.Exceptions
{
    public class CVDoesNotExistException : BaseRequestException
    {
        public CVDoesNotExistException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
