using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using EPiServer;
using EPiServer.Core.Html;
using EPiServer.Web;

namespace Geta.EPi.Cms.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtml(this string htmlText, int maxLength = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return htmlText;
            }

            return TextIndexer.StripHtml(htmlText, maxLength);
        }

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

        public static Url ToUrl(this string target)
        {
            return new Url(target);
        }

        /// <summary>
        ///     Creates URL / Html friendly slug
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string phrase, int maxLength = 50)
        {
            var charList = new List<char>();

            //seperate words in camel case
            for (int pos = 0; pos < phrase.Length; pos++)
            {
                var ch1 = phrase[pos];
                charList.Add(ch1);

                if (pos < phrase.Length - 1)
                {
                    var ch2 = phrase[pos + 1];

                    if (ch1 >= 'a' && ch1 <= 'z')
                    {
                        if (ch2 >= 'A' && ch2 <= 'Z')
                        {
                            charList.Add('-');
                        }
                    }
                }
            }

            string str = new string(charList.ToArray());

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


        public static int? TryParseInt32(this string input)
        {
            int outValue;
            return Int32.TryParse(input, out outValue) ? (int?)outValue : null;
        }

        public static long? TryParseInt64(this string input)
        {
            long outValue;
            return long.TryParse(input, out outValue) ? (long?)outValue : null;
        }
    }
}