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
    }
}