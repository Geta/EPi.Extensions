using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.ServiceLocation;
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
        ///     Uses HttpContext if available, otherwise uses EPiServer SiteDefinition SiteUrl.
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
        ///     Gets the friendly URL for the EPiServer permanent link.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <c>url</c> is null.</exception>
        public static string GetFriendlyUrl(this Url url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (IsMailTo(url))
            {
                return url.ToString();
            }

            var resolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            return resolver.GetUrl(url.ToString());
        }

        private static bool IsMailTo(Url url) => url.Scheme == "mailto";

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
            return HttpContext.Current == null
                ? SiteDefinition.Current.SiteUrl
                : new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
        }
    }
}
