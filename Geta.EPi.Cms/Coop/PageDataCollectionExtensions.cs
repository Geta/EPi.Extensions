using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;

namespace Geta.EPi.Cms.Coop
{
    public static class PageDataCollectionExtensions
    {
        public static PageDataCollection FilterAccess(this PageDataCollection pageDataCollection, AccessLevel accessLevel)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterAccess(accessLevel).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterCompareTo(this PageDataCollection pageDataCollection, string propertyName, string propertyValue)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterCompareTo(propertyName, propertyValue).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterCount(this PageDataCollection pageDataCollection, int count)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterCount(count).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterForVisitor(this PageDataCollection pageDataCollection, bool keepPagesWithoutTemplate)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterPublished().Filter(pageDataCollection);
            new FilterAccess().Filter(pageDataCollection);

            if (!keepPagesWithoutTemplate)
            {
                new FilterTemplate().Filter(pageDataCollection);
            }

            return pageDataCollection;
        }

        public static PageDataCollection FilterForVisitor(this PageDataCollection pageDataCollection)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            return EPiServer.Filters.FilterForVisitor.Filter(pageDataCollection);
        }

        public static PageDataCollection FilterPropertySort(this PageDataCollection pageDataCollection, string propertyName)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterPropertySort(propertyName).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterPropertySort(
                this PageDataCollection pageDataCollection,
                string propertyName,
                FilterSortDirection filterSortDirection)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterPropertySort(propertyName, filterSortDirection).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterPublished(this PageDataCollection pageDataCollection, PagePublishedStatus pagePublishedStatus)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterPublished(pagePublishedStatus).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterRemoveNullValues(this PageDataCollection pageDataCollection, string propertyName)
        {
            if (pageDataCollection == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            new FilterRemoveNullValues(propertyName).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterSkipCount(this PageDataCollection pageDataCollection, int skipCount)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterSkipCount(skipCount).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterSort(this PageDataCollection pageDataCollection, FilterSortOrder sortOrder)
        {
            if (pageDataCollection == null || sortOrder == FilterSortOrder.None)
            {
                return pageDataCollection;
            }

            new FilterSort(sortOrder).Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static PageDataCollection FilterTemplate(this PageDataCollection pageDataCollection)
        {
            if (pageDataCollection == null)
            {
                return null;
            }

            new FilterTemplate().Filter(pageDataCollection);

            return pageDataCollection;
        }

        public static bool HasChildren(this PageData pageData)
        {
            if (!pageData.IsEPiServerPage())
            {
                return false;
            }

            return pageData.GetChildren()
                    .FilterForVisitor()
                    .Any(page => page.VisibleInMenu);
        }
    }
}
