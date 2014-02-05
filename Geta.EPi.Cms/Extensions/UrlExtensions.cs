using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EPi.Cms.Extensions
{
    public static class UrlExtensions
    {
        /// <summary>
        /// Creates external Uri from provided Url. Uses HttpContext if available, othervise uses EPiServer settings' SiteUrl
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>External Uri for Url</returns>
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

        public static IHtmlString ToIHtmlString(this Url url)
        {
            if (url == null)
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(url.ToString());
        }

        public static string Resolve(this Url url)
        {
            return url.Resolve(null);
        }

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
                return EPiServer.Configuration.Settings.Instance.SiteUrl;
            }

            return new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
        }
    }
}
