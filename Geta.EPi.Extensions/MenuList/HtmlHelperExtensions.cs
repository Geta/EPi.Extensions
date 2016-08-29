using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;

namespace Geta.EPi.Extensions.MenuList
{
    /// <summary>
    ///     HtmlHelper extension for building menu list.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        ///     Returns an element for each child page of the rootLink using the itemTemplate.
        /// </summary>
        /// <param name="helper">The html helper in whose context the list should be created</param>
        /// <param name="rootLink">A reference to the root whose children should be listed</param>
        /// <param name="itemTemplate">
        ///     A template for each page which will be used to produce the return value. Can be either a
        ///     delegate or a Razor helper.
        /// </param>
        /// <param name="includeRoot">Whether an element for the root page should be returned</param>
        /// <param name="requireVisibleInMenu">
        ///     Wether pages that do not have the "Display in navigation" checkbox checked should be
        ///     excluded
        /// </param>
        /// <param name="requirePageTemplate">Whether page that do not have a template (i.e. container pages) should be excluded</param>
        /// <remarks>
        ///     Filter by access rights and publication status.
        /// </remarks>
        public static IHtmlString MenuList(this HtmlHelper helper, ContentReference rootLink, Func<MenuItem, HelperResult> itemTemplate = null,
            bool includeRoot = false, bool requireVisibleInMenu = true, bool requirePageTemplate = true)
        {
            var template = new Func<MenuItem<PageData>, HelperResult>(x =>
            {
                var menuItem = new MenuItem
                {
                    Content = x.Content,
                    Selected = x.Selected,
                    HasChildren = x.HasChildren,
                    HasSelectedChildContent = x.HasSelectedChildContent
                };
                return itemTemplate?.Invoke(menuItem);
            });

            return MenuList(helper, rootLink, template, includeRoot, requireVisibleInMenu, requirePageTemplate);
        }

        /// <summary>
        ///     Returns an element for each child page of the rootLink using the itemTemplate.
        /// </summary>
        /// <param name="helper">The html helper in whose context the list should be created</param>
        /// <param name="rootLink">A reference to the root whose children should be listed</param>
        /// <param name="itemTemplate">
        ///     A template for each page which will be used to produce the return value. Can be either a
        ///     delegate or a Razor helper.
        /// </param>
        /// <param name="includeRoot">Whether an element for the root page should be returned</param>
        /// <param name="requireVisibleInMenu">
        ///     Wether pages that do not have the "Display in navigation" checkbox checked should be
        ///     excluded
        /// </param>
        /// <param name="requireTemplate">Whether page that do not have a template (i.e. container pages) should be excluded</param>
        /// <remarks>
        ///     Filter by access rights and publication status.
        /// </remarks>
        public static IHtmlString MenuList<T>(this HtmlHelper helper, ContentReference rootLink, Func<MenuItem<T>, HelperResult> itemTemplate = null, bool includeRoot = false, bool requireVisibleInMenu = true, bool requireTemplate = true) where T : IContent
        {
            itemTemplate = itemTemplate ?? GetDefaultItemTemplate<T>(helper);
            var currentContentLink = helper.ViewContext.RequestContext.GetContentLink();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            Func<IEnumerable<T>, IEnumerable<T>> filter = contents => contents.FilterForDisplay(requireTemplate, requireVisibleInMenu);

            var menuItems = contentLoader.GetChildren<T>(rootLink)
                .FilterForDisplay(requireTemplate, requireVisibleInMenu)
                .Select(x => CreateMenuItem(x, currentContentLink, rootLink, contentLoader, filter))
                .ToList();

            if (includeRoot)
            {
                menuItems.Insert(0, CreateMenuItem(contentLoader.Get<T>(rootLink), currentContentLink, rootLink, contentLoader, filter));
            }

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            foreach (var menuItem in menuItems)
            {
                itemTemplate(menuItem).WriteTo(writer);
            }

            return new MvcHtmlString(buffer.ToString());
        }

        private static MenuItem<T> CreateMenuItem<T>(T content, ContentReference currentContentLink, ContentReference rootLink, IContentLoader contentLoader, Func<IEnumerable<T>, IEnumerable<T>> filter) where T : IContent
        {
            var menuItem = new MenuItem<T>
            {
                Content = content,
                Selected = content.ContentLink.CompareToIgnoreWorkID(currentContentLink),
                HasChildren = new Lazy<bool>(() => filter(contentLoader.GetChildren<T>(content.ContentLink)).Any()),
                HasSelectedChildContent = new Lazy<bool>(() => HasSelectedChildContent(rootLink, currentContentLink, content))
            };
            return menuItem;
        }

        private static bool HasSelectedChildContent<T>(ContentReference rootLink, ContentReference currentContentLink, T content) where T : IContent
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            var contentPath = contentLoader.GetAncestors(currentContentLink)
                .Reverse()
                .Select(x => x.ContentLink)
                .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
                .ToList();

            return contentPath.Contains(content.ContentLink);
        }

        private static Func<MenuItem<T>, HelperResult> GetDefaultItemTemplate<T>(HtmlHelper helper) where T : IContent
        {
            return x => new HelperResult(writer => writer.Write(helper.ContentLink(x.Content)));
        }
    }
}