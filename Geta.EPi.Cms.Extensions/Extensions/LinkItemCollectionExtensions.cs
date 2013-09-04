using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace Geta.EPi.Cms.Extensions
{
    public static class LinkItemCollectionExtensions
    {
        /// <summary>
        /// Get a PageDataCollection with all the EPiServer pages in a LinkItemCollection
        /// </summary>
        /// <param name="linkItemCollection"></param>
        /// <returns>All the EPiServer pages in a LinkItemCollection</returns>
        public static PageDataCollection ToPageDataCollection(this LinkItemCollection linkItemCollection)
        {
            if (linkItemCollection == null)
            {
                return null;
            }

            var pageDataCollection = new PageDataCollection();
            foreach (var linkItem in linkItemCollection)
            {
                var url = new UrlBuilder(linkItem.Href);
                bool isEPiServerPage = PermanentLinkMapStore.ToMapped(url);

                if (isEPiServerPage)
                {
                    var page = ServiceLocator.Current.GetInstance<IContentLoader>().Get<PageData>(PermanentLinkUtility.GetContentReference(url));

                    if (page != null)
                    {
                        pageDataCollection.Add(page);
                    }
                }
            }

            return pageDataCollection;
        }

        /// <summary>
        /// Gets a IEnumerable with all the EPiServer pages of given type T in a LinkItemCollection
        /// </summary>
        /// <param name="linkItemCollection"></param>
        /// <returns>All the EPiServer pages of type T in a LinkItemCollection</returns>
        public static IEnumerable<T> ToEnumerable<T>(this LinkItemCollection linkItemCollection) where T : PageData
        {
            if (linkItemCollection == null)
            {
                yield break;
            }

            foreach (var linkItem in linkItemCollection)
            {
                var url = new UrlBuilder(linkItem.Href);
                var isEPiServerPage = PermanentLinkMapStore.ToMapped(url);

	            if (!isEPiServerPage) 
					continue;

	            var page = ServiceLocator.Current.GetInstance<IContentLoader>().Get<PageData>(PermanentLinkUtility.GetContentReference(url)) as T;

	            if (page != null)
	            {
		            yield return page;
	            }
            }
        }
    }
}