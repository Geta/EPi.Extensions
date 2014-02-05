using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Filters;

namespace Geta.EPi.Cms.Kolumbus
{
    public static class EnumerableOfPageDataExtensions
    {
        public static IEnumerable<PageData> Sort(this IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);

            return asCollection;
        }
    }
}
