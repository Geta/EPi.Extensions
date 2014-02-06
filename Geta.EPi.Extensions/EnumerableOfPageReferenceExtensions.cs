using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    public static class EnumerableOfPageReferenceExtensions
    {
        /// <summary>
        /// Converts sequence of PageReference to PageDataCollection.
        /// </summary>
        /// <param name="pageReferences">Source sequence of PageReference to convert.</param>
        /// <returns>Instance of PageDataCollection from source sequence.</returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageReference> pageReferences)
        {
            return new PageDataCollection(pageReferences);
        }
    }
}