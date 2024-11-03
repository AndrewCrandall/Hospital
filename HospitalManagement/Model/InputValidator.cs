// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : InputValidator
// Purpose : Provide the logic for checking all text box fields

using System;
using System.Text.RegularExpressions;
using System.Web;

namespace HospitalManagement.Utilities
{
    public static class InputValidator
    {
        public static string SanitizeInput(string input)
        {
            return Regex.Replace(HttpUtility.HtmlEncode(input), "<.*?>", string.Empty).Trim();
        }

        public static bool IsValidInput(string username, string password)
        {
            const int minLength = 5;
            const int maxLength = 20;
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) &&
                   username.Length >= minLength && username.Length <= maxLength &&
                   password.Length >= minLength && password.Length <= maxLength &&
                   Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"); // Alphanumeric and underscores
        }

        public static bool IsValidMfaCode(string mfaCode)
        {
            // MFA code must be exactly six digits
            return mfaCode.Length == 6 && Regex.IsMatch(mfaCode, @"^\d{6}$");
        }
    }
}
