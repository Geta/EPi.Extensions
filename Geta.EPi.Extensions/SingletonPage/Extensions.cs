using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions.SingletonPage
{
    /// <summary>
    /// Singleton page extensions.
    /// </summary>
    public static class Extensions
    {
        private static readonly IContentReferenceCache DefaultContentReferenceCache 
                                                            = new DefaultContentReferenceCache();

        /// <summary>
        /// Injected IContentLoader.
        /// </summary>
        public static Injected<IContentLoader> InjectedContentLoader { get; set; }
        private static IContentLoader ContentLoader => InjectedContentLoader.Service;
        /// <summary>
        /// Injected IContentReferenceCache.
        /// </summary>
        public static Injected<IContentReferenceCache> InjectedCache { get; set; }
        private static IContentReferenceCache Cache => InjectedCache.Service ?? DefaultContentReferenceCache;

        /// <summary>
        /// Returns first page of a given type under the root rootPage.
        /// </summary>
        /// <param name="rootPage">The root page under which to search.</param>
        /// <typeparam name="T">Type of the page to search for.</typeparam>
        /// <returns>The first page of provided type if found; otherwise null.</returns>
        public static T GetSingletonPage<T>(this PageData rootPage)
            where T : PageData, new()
        {
            return rootPage.ContentLink.GetSingletonPage<T>();
        }

        /// <summary>
        /// Returns first page of a given type under the root rootPage by root rootPage's content reference.
        /// </summary>
        /// <param name="rootPageLink">The root page's content reference under which to search.</param>
        /// <typeparam name="T">Type of the page to search for.</typeparam>
        /// <returns>The first page of provided type if found; otherwise null.</returns>
        public static T GetSingletonPage<T>(this ContentReference rootPageLink)
            where T : PageData, new()
        {
            var singletonLink = Cache.GetOrAdd(new CacheKey(typeof(T), rootPageLink), rootPageLink.GetSingletonPageLink<T>);
            return ContentLoader.Get<T>(singletonLink);
        }

        /// <summary>
        /// Returns first page's of a given type content reference under the root rootPage by root rootPage's content reference.
        /// </summary>
        /// <param name="rootPageLink">The root page's content reference under which to search.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ContentReference GetSingletonPageLink<T>(this ContentReference rootPageLink)
            where T : PageData, new()
        {
            var singletonPage = ContentLoader.GetDescendents(rootPageLink)
                .Select(ContentLoader.Get<IContent>)
                .OfType<T>()
                .FirstOrDefault();

            return singletonPage != null ? singletonPage.ContentLink : ContentReference.EmptyReference;
        }
    }
}