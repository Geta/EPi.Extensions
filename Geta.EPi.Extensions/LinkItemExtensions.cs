using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Extensions
{
    public static class LinkItemExtensions
    {
        /// <summary>
        /// Returns ContentReference for provided LinkItem if it is EPiServer page otherwise returns EmptyReference.
        /// </summary>
        /// <param name="source">Source LinkItem for which to return content reference.</param>
        /// <returns>Returns ContentReference for provided LinkItem if it is EPiServer page otherwise returns EmptyReference.</returns>
        public static ContentReference ToContentReference(this LinkItem source)
        {
            var urlBuilder = new UrlBuilder(source.Href);
            return PermanentLinkMapStore.ToMapped(urlBuilder) 
                ? PermanentLinkUtility.GetContentReference(urlBuilder) 
                : ContentReference.EmptyReference;
        }
    }
}
