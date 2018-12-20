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
            var context = HttpContext.Current != null ? new HttpContextWrapper(HttpContext.Current) : null;
            return GetBaseUri(context, SiteDefinition.Current);
        }

        /// <summary>
        /// Returns base URI for the site.
        /// </summary>
        /// <returns>Base site URI</returns>
        public static Uri GetBaseUri(HttpContextBase context, SiteDefinition siteDefinition)
        {
            var siteUri = context != null
                ? context.Request.Url
                : siteDefinition.SiteUrl;

            var scheme = context != null && !string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-Proto"])
                ? context.Request.Headers["X-Forwarded-Proto"].Split(',')[0]
                : siteUri?.Scheme;

            var urlBuilder = new UrlBuilder(siteUri)
            {
                Scheme = scheme ?? "https"
            };
            return urlBuilder.Uri;
        }
    }
}