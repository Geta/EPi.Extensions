using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;

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
