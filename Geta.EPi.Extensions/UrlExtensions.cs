using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Url extensions.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        ///     Creates external Uri from provided Url.
        ///     Uses HttpContext if available, othervise uses EPiServer SiteDefinition SiteUrl.
        /// </summary>
        /// <param name="url">Url for which to create Uri.</param>
        /// <returns>External Uri for Url.</returns>
        public static Uri ToAbsoluteUri(this Url url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (url.IsEmpty())
            {
                throw new ArgumentException("Provided URL is empty");
            }

            if (url.IsAbsoluteUri)
            {
                return url.Uri;
            }

            var baseUri = GetBaseUri();
            return new Uri(baseUri, url.Uri);
        }

        /// <summary>
        ///     Creates Html string of provided Url.
        /// </summary>
        /// <param name="url">Url for which to create Html string.</param>
        /// <returns>Html string with Url if Url is not null. Otherwise returns empty string.</returns>
        public static IHtmlString ToIHtmlString(this Url url)
        {
            return url == null
                ? MvcHtmlString.Empty
                : MvcHtmlString.Create(url.ToString());
        }

        private static Uri GetBaseUri()
        {
            if (HttpContext.Current == null)
            {
                return SiteDefinition.Current.SiteUrl;
            }

            return new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
        }
    }
}