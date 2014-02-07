using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public static string StripHtml(this string htmlText, int maxLength = int.MaxValue)
        {
            return string.IsNullOrWhiteSpace(htmlText)
                ? htmlText
                : TextIndexer.StripHtml(htmlText, maxLength);
        }

        /// <summary>
        ///     Returns absolute URL for given EPiServer <paramref name="permanentlink" />.
        /// </summary>
        /// <param name="permanentlink">Permanent URL of EPiServer content.</param>
        /// <returns>UrlBuilder instance which represents absolute URL.</returns>
        public static UrlBuilder GetExternalUrl(this string permanentlink)
        {
            if (string.IsNullOrWhiteSpace(permanentlink))
            {
                return null;
            }

            var url = new UrlBuilder(new UrlBuilder(permanentlink));

            PermanentLinkMapStore.ToMapped(url);

            url.Host = HttpContext.Current.Request.Url.Host;
            url.Scheme = HttpContext.Current.Request.Url.Scheme;
            url.Port = HttpContext.Current.Request.Url.Port;

            return url;
        }

        /// <summary>
        ///     Converts string URL <paramref name="source"/> to Url instance.
        /// </summary>
        /// <param name="source">String URL.</param>
        /// <returns>Instance of Url class which represents <paramref name="source"/> URL.</returns>
        public static Url ToUrl(this string source)
        {
            return new Url(source);
        }

        /// <summary>
        ///     Creates URL / Html friendly slug.
        /// </summary>
        /// <param name="phrase">Phrease to create slug from.</param>
        /// <param name="maxLength">Maximum length of the slug.</param>
        /// <returns>Created string slug.</returns>
        public static string GenerateSlug(this string phrase, int maxLength = 50)
        {
            var charList = new List<char>();

            //seperate words in camel case
            for (var pos = 0; pos < phrase.Length; pos++)
            {
                var ch1 = phrase[pos];
                charList.Add(ch1);

                if (pos >= phrase.Length - 1) continue;

                var ch2 = phrase[pos + 1];

                if (ch1 < 'a' || ch1 > 'z') continue;

                if (ch2 >= 'A' && ch2 <= 'Z')
                {
                    charList.Add('-');
                }
            }

            var str = new string(charList.ToArray());

            str = str.ToLowerInvariant();

            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        /// <summary>
        /// Parses string to nullable int (Int32).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>int (Int32) value if parse succeeds otherwise null.</returns>
        public static int? TryParseInt32(this string input)
        {
            int outValue;
            return Int32.TryParse(input, out outValue) ? (int?) outValue : null;
        }

        /// <summary>
        /// Parses string to nullable long (Int64).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>long (Int64) value if parse succeeds otherwise null.</returns>
        public static long? TryParseInt64(this string input)
        {
            long outValue;
            return long.TryParse(input, out outValue) ? (long?) outValue : null;
        }
    }
}