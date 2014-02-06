using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Cms.Extensions
{
    public static class LinkItemExtensions
    {
        public static ContentReference GetContentReference(this LinkItem target)
        {
            var urlBuilder = new UrlBuilder(target.Href);
            return PermanentLinkMapStore.ToMapped(urlBuilder) 
                ? PermanentLinkUtility.GetContentReference(urlBuilder) 
                : ContentReference.EmptyReference;
        }
    }
}
