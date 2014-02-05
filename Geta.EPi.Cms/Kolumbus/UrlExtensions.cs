using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EPi.Cms.Kolumbus
{
    public static class UrlExtensions
    {
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
            if (reference == ContentReference.EmptyReference)
            {
                return url.ToString();
            }

            var resolver = new UrlResolver();
            return resolver.GetVirtualPath(reference, language);
        }
    }
}
