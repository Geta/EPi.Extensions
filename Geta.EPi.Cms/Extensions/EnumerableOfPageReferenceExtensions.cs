using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.EPi.Cms.Extensions
{
    public static class EnumerableOfPageReferenceExtensions
    {
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageReference> pageReferences)
        {
            return new PageDataCollection(pageReferences);
        }
    }
}