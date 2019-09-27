using System;
using EPiServer.Core.Html;
using Geta.EPi.Extensions.Helpers;
using Geta.Net.Extensions;

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
        /// Adds scheme and host to a relative URL.
        /// </summary>
        /// <param name="input">URL</param>
        /// <returns>Returns URL with scheme and host.</returns>
        public static string GetExternalUrl(this string input)
        {
            return AddHost(input);
        }

        /// <summary>
        /// Removes scheme and host from the URL.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Returns URL without scheme and host.</returns>
        public static string RemoveHost(this string url)
        {
            if (url.IsAbsoluteUrl())
            {
                var uri = new Uri(url);
                return uri.PathAndQuery;
            }
            return url ?? string.Empty;
        }

        /// <summary>
        /// Adds scheme and host to a relative URL. Uses UriHelpers.GetBaseUri() to retrieve base URL for the scheme and host.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Returns URL with scheme and host.</returns>
        public static string AddHost(this string url)
        {
            return url.AddHost(UriHelpers.GetBaseUri);
        }

        /// <summary>
        /// Adds scheme and host to a relative URL.
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="getBaseUri">Function which returns base URL.</param>
        /// <returns>Returns URL with scheme and host.</returns>
        public static string AddHost(this string url, Func<Uri> getBaseUri)
        {
            var baseUri = getBaseUri();
            var uri = new Uri(baseUri, url).ToString();
            return
                new UriBuilder(uri) { Scheme = baseUri.Scheme, Port = -1 }
                .ToString();
        }
    }
}