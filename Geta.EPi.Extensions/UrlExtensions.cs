using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

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
        public static Uri ToExternalUri(this Url url)
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

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Resolve(this Url url)
        {
            return url.Resolve(null);
        }

        /// <summary>
        ///     Returns string representation of URL for provided Url instance and language.
        /// </summary>
        /// <param name="url">Url for which to create string URL.</param>
        /// <param name="language">Language code for which to create URL.</param>
        /// <returns>
        ///     String representation of URL. If Url represents EPiServer page it creates Url with provided language otherwise
        ///     returns string representation of <paramref name="url" />
        /// </returns>
        public static string Resolve(this Url url, string language)
        {
            if (url == null)
            {
                return null;
            }

            var reference = PermanentLinkUtility.GetContentReference(new UrlBuilder(url));
            if (ContentReference.IsNullOrEmpty(reference))
            {
                return url.ToString();
            }

            var resolver = new UrlResolver();
            return resolver.GetUrl(reference, language);
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