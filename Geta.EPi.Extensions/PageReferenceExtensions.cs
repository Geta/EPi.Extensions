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
    }
}