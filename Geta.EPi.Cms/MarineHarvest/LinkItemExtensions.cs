using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class LinkItemExtensions
    {
        public static ContentReference GetContentReference(this LinkItem target)
        {
            var result = ContentReference.EmptyReference;
            var urlBuilder = new UrlBuilder(target.Href);
            return PermanentLinkMapStore.ToMapped(urlBuilder) ? PermanentLinkUtility.GetContentReference(urlBuilder) : result;
        }
    }
}
