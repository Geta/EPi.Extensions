using System;
using System.Web;
using EPiServer;

namespace Geta.EPi.Cms.Extensions
{
    public static class UrlExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns></returns>
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
