using System.Net.Mail;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    ///     Helper methods for validation
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        ///     Verifies if is valid email
        /// </summary>
        /// <param name="email">Email string</param>
        /// <returns>true if valid email, false if not valid email</returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}