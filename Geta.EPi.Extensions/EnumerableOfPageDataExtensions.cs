using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Filters;

namespace Geta.EPi.Extensions
{
    public static class EnumerableOfPageDataExtensions
    {
        /// <summary>
        /// Sorts sequence of PageData based on FilterSortOrder.
        /// </summary>
        /// <param name="pages">Source sequence of PageData.</param>
        /// <param name="sortOrder">FilterSortOrder value which indicates sort order.</param>
        /// <returns>Sorted sequence of PageData.</returns>
        public static IEnumerable<PageData> Sort(this IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = pages.ToPageDataCollection();
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection;
        }

        /// <summary>
        /// Converts sequence of PageData to PageDataCollection.
        /// </summary>
        /// <param name="pages">Source sequence of PageData to convert.</param>
        /// <returns>Instance of PageDataCollection from source sequence.</returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageData> pages)
        {
            return new PageDataCollection(pages);
        }
    }
}
