using System.Web.WebPages;
using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web.Routing;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     LinkItem Extensions.
    /// </summary>
    public static class LinkItemExtensions
    {
        /// <summary>
        ///     Returns ContentReference for provided LinkItem if it is EPiServer content otherwise returns EmptyReference.
        /// </summary>
        /// <param name="source">Source LinkItem for which to return content reference.</param>
        /// <returns>Returns ContentReference for provided LinkItem if it is EPiServer content otherwise returns EmptyReference. Note: do not use this to later on generate url as remaining url part will be lost</returns>
        public static ContentReference ToContentReference(this LinkItem source)
        {
            var content = source.ToContent();

            return content != null
                ? content.ContentLink
                : ContentReference.EmptyReference;
        }

        /// <summary>
        ///     Returns IContent for provided LinkItem if it is EPiServer content otherwise returns null.
        /// </summary>
        /// <param name="source">Source LinkItem for which to return content.</param>
        /// <returns>Returns IContent for provided LinkItem if it is EPiServer content otherwise returns null. Note: do not use this to later on generate url as remaining url part will be lost</returns>
        public static IContent ToContent(this LinkItem source)
        {
            var urlBuilder = new UrlBuilder(source.GetMappedHref());

            return UrlResolver.Current.Route(urlBuilder);
        }

        /// <summary>
        ///     Returns friendly URL if item is EPiServer content, otherwise returns the original Href property value.
        /// </summary>
        /// <param name="linkItem">Source LinkItem for which to return external URL.</param>
        /// <param name="includeHost">Mark if include host name in the url, unless it is external url then it still will contain absolute url</param>
        /// <returns>Returns friendly URL if item is EPiServer content, otherwise returns the original Href property value.</returns>
        public static string GetFriendlyUrl(this LinkItem linkItem, bool includeHost = false)
        {
            if (string.IsNullOrWhiteSpace(linkItem.Href))
            {
                return string.Empty;
            }

            var url = new Url(linkItem.GetMappedHref());
            if (url.IsEmpty())
            {
                return string.Empty;
            }

            var friendlyUrl = url.IsAbsoluteUri
                ? url.ToString()
                : UrlResolver.Current.GetUrl(url.ToString()) ?? url.ToString();

            return includeHost && !string.IsNullOrWhiteSpace(friendlyUrl) ? friendlyUrl.AddHost() : friendlyUrl;
        }
    }
}