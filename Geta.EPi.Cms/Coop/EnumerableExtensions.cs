using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.EPi.Cms.Coop
{
    public static class EnumerableExtensions
    {
        

        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageReference> pageReferences)
        {
            return new PageDataCollection(pageReferences);
        }
    }
}