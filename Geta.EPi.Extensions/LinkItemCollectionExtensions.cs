using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Extensions
{
    public static class LinkItemCollectionExtensions
    {
        /// <summary>
        ///     Returns a PageDataCollection with all the EPiServer pages from a LinkItemCollection.
        /// </summary>
        /// <param name="linkItemCollection">Source LinkItemCollection to look for EPiServer pages.</param>
        /// <returns>PageDataCollection with EPiServer pages from a LinkItemCollection.</returns>
        public static PageDataCollection ToPageDataCollection(this LinkItemCollection linkItemCollection)
        {
            return new PageDataCollection(linkItemCollection.ToEnumerable<PageData>());
        }

        /// <summary>
        ///     Returns a sequence with all the EPiServer pages of given type <typeparamref name="T"/> in a LinkItemCollection
        /// </summary>
        /// <param name="linkItemCollection">Source LinkItemCollection to look for EPiServer pages.</param>
        /// <returns>Sequence of the EPiServer pages of type <typeparamref name="T"/> in a LinkItemCollection</returns>
        public static IEnumerable<T> ToEnumerable<T>(this LinkItemCollection linkItemCollection) 
            where T : PageData
        {
            if (linkItemCollection == null)
            {
                return Enumerable.Empty<T>();
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return linkItemCollection
                .Select(x=> x.ToContentReference())
                .Where(x => !x.IsNullOrEmpty())
                .Select(contentLoader.Get<T>);
        }
    }
}