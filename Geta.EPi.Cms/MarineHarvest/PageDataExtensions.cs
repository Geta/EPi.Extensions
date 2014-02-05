using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Core;
using Geta.EPi.Cms.Extensions;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class PageDataExtensions
    {
        public static IEnumerable<PageData> GetPageDataPath(this PageData node)
        {
            var result = new List<PageData>();
            PageData cursor = node;

            while (cursor != null)
            {
                result.Add(cursor);
                cursor = cursor.GetParent();
            }
            return result;
        }

        public static IEnumerable<T> GetPageDataPath<T>(this PageData node)
        {
            return GetPageDataPath(node)
                .Where(p => p is T)
                .Cast<T>();
        }

        public static TPropertyType GetInheritedProperty<TPageType, TPropertyType>(
            this TPageType pageType,
            Expression<Func<TPageType, TPropertyType>> expression)
            where TPageType : PageData
            where TPropertyType : class
        {
            TPropertyType result = pageType.GetPropertyValue(expression);
            if (result != null)
            {
                return result;
            }

            var parent = pageType.GetParent() as TPageType;
            if (parent != null)
            {
                result = parent.GetInheritedProperty(expression);
            }

            return result;
        }

        public static bool IsPageEqual(this PageData page1, PageData page2)
        {
            if (page1 == page2)
            {
                return true;
            }

            return page1 != null && page2 != null && page1.ContentLink.CompareToIgnoreWorkID(page2.ContentLink);
        }
    }
}