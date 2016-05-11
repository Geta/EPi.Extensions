using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions.SingletonPage
{
    public static class Extensions
    {
        private static readonly IContentReferenceCache DefaultContentReferenceCache 
                                                            = new DefaultContentReferenceCache();

        public static Injected<IContentLoader> InjectedContentLoader { get; set; }
        private static IContentLoader ContentLoader => InjectedContentLoader.Service;
        public static Injected<IContentReferenceCache> InjectedCache { get; set; }
        private static IContentReferenceCache Cache => InjectedCache.Service ?? DefaultContentReferenceCache;

        public static T GetSingletonPage<T>(this PageData page)
            where T : PageData, new()
        {
            return page.ContentLink.GetSingletonPage<T>();
        }

        public static T GetSingletonPage<T>(this ContentReference pageLink)
            where T : PageData, new()
        {
            var singletonLink = Cache.GetOrAdd(new CacheKey(typeof(T), pageLink), pageLink.GetSingletonPageLink<T>);
            return ContentLoader.Get<T>(singletonLink);
        }

        public static ContentReference GetSingletonPageLink<T>(this ContentReference rootPageLink)
            where T : PageData, new()
        {
            var page = ContentLoader.GetDescendents(rootPageLink)
                .Select(ContentLoader.Get<PageData>)
                .OfType<T>()
                .FirstOrDefault();

            return page != null ? page.ContentLink : ContentReference.EmptyReference;
        }
    }
}