using System.Text.RegularExpressions;
using System.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// HttpRequest extensions.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Checks if current request is a crawler request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsCrawler(this HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserAgent))
                return false;

            return Regex.IsMatch(
                request.UserAgent,
                @"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google",
                RegexOptions.IgnoreCase);
        }
    }
}
