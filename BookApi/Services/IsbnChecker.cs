using System.Text.RegularExpressions;

namespace Books.Services
{
    /// <summary>
    /// Checks if isbn number is valid
    /// Code taken from
    /// https://www.geeksforgeeks.org/regular-expressions-to-validate-isbn-code/
    /// </summary>
    public class IsbnChecker : IIsbnChecker
    {
        public bool isValidISBNCode(string str)
        {
            string strRegex
                = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(str))
                return (true);
            else
                return (false);
        }


    }
}
