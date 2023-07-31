using System.Text.RegularExpressions;

namespace JobManagementApi.Validations.StaticPredicates
{
    public static class Predicates
    {
        public static bool NotNullOrEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsCorrectEmailFormat(string value)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(value);
        }
    }
}
