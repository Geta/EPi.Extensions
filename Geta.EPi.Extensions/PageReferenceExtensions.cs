using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     PageReference extensions.
    /// </summary>
    public static class PageReferenceExtensions
    {
        /// <summary>
        ///     Indicates whether the specified page reference is null or an EmptyReference.
        /// </summary>
        /// <param name="pageReference">Page reference to test.</param>
        /// <returns>true if page reference is null or EmptyReference else false</returns>
        public static bool IsNullOrEmpty(this PageReference pageReference)
        {
            return PageReference.IsNullOrEmpty(pageReference);
        }

        /// <summary>
        ///     Converts sequence of PageReference to PageDataCollection.
        /// </summary>
        /// <param name="pageReferences">Source sequence of PageReference to convert.</param>
        /// <returns>Instance of PageDataCollection from source sequence.</returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageReference> pageReferences)
        {
            return new PageDataCollection(pageReferences);
        }

        /// <summary>
        /// Gets the canonical URL for a page. If LinkType is FetchData the original page URL will be the canonical link.
        /// </summary>
        /// <param name="pageLink">The PageReference to get canonical URL from.</param>
        /// <param name="considerFetchDataFrom">Consider fetch data from setting in EPiServer.</param>
        /// <returns></returns>
        public static string GetCanonicalUrl(this PageReference pageLink, bool considerFetchDataFrom = true)
        {
            if (PageReference.IsNullOrEmpty(pageLink))
            {
                return null;
            }

            PageData page = pageLink.GetPage();

            if (page == null)
            {
                return null;
            }

            return page.GetCanonicalUrl(considerFetchDataFrom);
        }
    }
}