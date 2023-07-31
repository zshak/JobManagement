namespace JobManagementApi.Models.Exceptions
{
    public class VacancyNotFoundException : BaseRequestException
    {
        public VacancyNotFoundException(string message, int statusCode) : base(message, statusCode)
        {
        }
    }
}
