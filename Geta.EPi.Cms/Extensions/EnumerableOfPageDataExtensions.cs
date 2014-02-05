using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Filters;

namespace Geta.EPi.Cms.Extensions
{
    public static class EnumerableOfPageDataExtensions
    {
        public static IEnumerable<PageData> Sort(this IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = pages.ToPageDataCollection();
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);

            return asCollection;
        }

        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageData> pages)
        {
            return new PageDataCollection(pages);
        }
    }
}
