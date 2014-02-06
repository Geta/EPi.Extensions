using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions
{
    public static class ContentExtensions
    {
        /// <summary>
        ///     Filters content which should not be visible to the user.
        /// </summary>
        /// <typeparam name="T">Type of content (IContent).</typeparam>
        /// <param name="content">Enumeration of content instances to filter.</param>
        /// <param name="requirePageTemplate">Mark if should include only content with template.</param>
        /// <param name="requireVisibleInMenu">Mark if should include only pages with mark VisibleInMenu = true.</param>
        /// <returns>Returns enumeration of filtered content.</returns>
        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> content,
            bool requirePageTemplate = false,
            bool requireVisibleInMenu = false)
            where T : IContent
        {
            if (content == null) return Enumerable.Empty<T>();

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var publishedFilter = new FilterPublished(contentRepository);
            var accessFilter = new FilterAccess();
            content = content.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));

            if (requirePageTemplate)
            {
                var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Page;
                content = content.Where(x => !templateFilter.ShouldFilter(x));
            }

            if (requireVisibleInMenu)
            {
                content = content.Where(x => VisibleInMenu(x));
            }

            return content;
        }

        private static bool VisibleInMenu(IContent content)
        {
            var page = content as PageData;
            return page == null || page.VisibleInMenu;
        }
    }
}