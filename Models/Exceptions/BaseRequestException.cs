namespace JobManagementApi.Models.Exceptions
{
    public class BaseRequestException : Exception
    {
        public int StatusCode { get; set; }
        public BaseRequestException(string message, int statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}