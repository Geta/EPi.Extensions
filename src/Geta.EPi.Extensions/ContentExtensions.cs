using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Content extensions.
    /// </summary>
    public static class ContentExtensions
    {
        /// <summary>
        /// Injected IContentLoader.
        /// </summary>
        public static Injected<IContentLoader> InjectedContentLoader { get; set; }
        private static IContentLoader ContentLoader => InjectedContentLoader.Service;

        /// <summary>
        ///     Filters content which should not be visible to the user.
        /// </summary>
        /// <typeparam name="T">Type of content (IContent).</typeparam>
        /// <param name="content">Enumeration of content instances to filter.</param>
        /// <param name="requirePageTemplate">Mark if should include only content with template.</param>
        /// <param name="requireVisibleInMenu">Mark if should include only pages with mark VisibleInMenu = true.</param>
        /// <returns>Returns enumeration of filtered content.</returns>
        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> content, bool requirePageTemplate = false, bool requireVisibleInMenu = false)
            where T : IContent
        {
            if (content == null) return Enumerable.Empty<T>();

            var publishedFilter = new FilterPublished();
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

        /// <summary>
        /// Returns first ancestor of type.
        /// </summary>
        /// <typeparam name="T">Type of content (IContent).</typeparam>
        /// <param name="instance">Content instance.</param>
        /// <returns>First ancestor or default of T.</returns>
        public static T ClosestAncestor<T>(this IContent instance)
            where T : IContent
        {
            return ContentLoader.GetAncestors(instance.ContentLink).OfType<T>().FirstOrDefault();
        }

        private static bool VisibleInMenu(IContent content)
        {
            var page = content as PageData;
            return page == null || page.VisibleInMenu;
        }
    }
}