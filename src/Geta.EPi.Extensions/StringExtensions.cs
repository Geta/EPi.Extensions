using System;
using System.Web;
using EPiServer;
using EPiServer.Core.Html;
using EPiServer.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Strip all HTML elements from a HTML string and return text.
        /// </summary>
        /// <param name="htmlText">A HTML string.</param>
        /// <param name="maxLength">Max string length to return. 0 returns all text in the HTML string.</param>
        /// <returns>A string with text.</returns>
        public static string StripHtml(this string htmlText, int maxLength = 0)
        {
            return string.IsNullOrWhiteSpace(htmlText)
                ? htmlText
                : TextIndexer.StripHtml(htmlText, maxLength);
        }

        /// <summary>
        ///     Parses string to nullable int (Int32).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>int (Int32) value if parse succeeds otherwise null.</returns>
        public static int? TryParseInt32(this string input)
        {
            int outValue;
            return Int32.TryParse(input, out outValue) ? (int?) outValue : null;
        }

        /// <summary>
        ///     Parses string to nullable long (Int64).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>long (Int64) value if parse succeeds otherwise null.</returns>
        public static long? TryParseInt64(this string input)
        {
            long outValue;
            return long.TryParse(input, out outValue) ? (long?) outValue : null;
        }

        /// <summary>
        /// Adds scheme and host to a relative url.
        /// </summary>
        /// <param name="input">Url</param>
        /// <returns>String</returns>
        public static string GetExternalUrl(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var siteUri = HttpContext.Current != null
                            ? HttpContext.Current.Request.Url
                            : SiteDefinition.Current.SiteUrl;

            var scheme = HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-Proto"])
                            ? HttpContext.Current.Request.Headers["X-Forwarded-Proto"]
                            : siteUri.Scheme;

            if (!input.StartsWith("/"))
            {
                input = "/" + input;
            }

            var urlBuilder = new UrlBuilder(input)
            {
                Scheme = scheme,
                Host = siteUri.Host,
                Port = siteUri.Port
            };

            return urlBuilder.ToString();
        }
    }
}