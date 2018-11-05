using System;
using System.Web;
using EPiServer;
using EPiServer.Web;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    /// Uri helpers
    /// </summary>
    public class UriHelpers
    {
        /// <summary>
        /// Returns base URI for the site.
        /// </summary>
        /// <returns>Base site URI</returns>
        public static Uri GetBaseUri()
        {
            var siteUri = HttpContext.Current != null
                ? HttpContext.Current.Request.Url
                : SiteDefinition.Current.SiteUrl;

            var scheme = HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-Proto"])
                ? HttpContext.Current.Request.Headers["X-Forwarded-Proto"].Split(',')[0]
                : siteUri.Scheme;

            var urlBuilder = new UrlBuilder(siteUri)
            {
                Scheme = scheme
            };
            return urlBuilder.Uri;
        }
    }
}
