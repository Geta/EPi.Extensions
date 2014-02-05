using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Cms.Kolumbus
{
    public static class LinkItemCollectionExtensions
    {
        public static IEnumerable<T> GetPageDataCollection<T>(this LinkItemCollection collection) where T : PageData
        {
            var pages = new List<T>();
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var references = collection.Select(li => PermanentLinkUtility.GetContentReference(new UrlBuilder(li.ToMappedLink())));

            foreach (var reference in references)
            {
                try
                {
                    pages.Add(loader.Get<T>(reference));
                }
                catch (TypeMismatchException)
                {
                }
            }

            return pages;
        }
    }
}
