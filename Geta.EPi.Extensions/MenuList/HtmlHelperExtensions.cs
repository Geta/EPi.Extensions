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
            itemTemplate = itemTemplate ?? GetDefaultItemTemplate(helper);
            var currentContentLink = helper.ViewContext.RequestContext.GetContentLink();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            Func<IEnumerable<PageData>, IEnumerable<PageData>> filter = pages => pages.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);
            
            var menuItems = contentLoader.GetChildren<PageData>(rootLink)
                .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
                .Select(x => CreateMenuItem(x, currentContentLink, rootLink, contentLoader, filter))
                .ToList();

            if (includeRoot)
            {
                menuItems.Insert(0, CreateMenuItem(contentLoader.Get<PageData>(rootLink), currentContentLink, rootLink, contentLoader, filter));
            }

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            foreach (var menuItem in menuItems)
            {
                itemTemplate(menuItem).WriteTo(writer);
            }

            return new MvcHtmlString(buffer.ToString());
        }

        private static MenuItem CreateMenuItem(PageData page, ContentReference currentContentLink, ContentReference rootLink, IContentLoader contentLoader, Func<IEnumerable<PageData>, IEnumerable<PageData>> filter)
        {
            var menuItem = new MenuItem
            {
                Page = page,
                Selected = page.ContentLink.CompareToIgnoreWorkID(currentContentLink),
                HasChildren = new Lazy<bool>(() => filter(contentLoader.GetChildren<PageData>(page.ContentLink)).Any()),
                HasSelectedChildPage = new Lazy<bool>(() => HasSelectedChildPage(rootLink, currentContentLink, page))
            };
            return menuItem;
        }

        private static bool HasSelectedChildPage(ContentReference rootLink, ContentReference currentContentLink, PageData page)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            List<ContentReference> pagePath = contentLoader.GetAncestors(currentContentLink)
                .Reverse()
                .Select(x => x.ContentLink)
                .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
                .ToList();

            return pagePath.Contains(page.ContentLink);
        }

        private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(HtmlHelper helper)
        {
            return x => new HelperResult(writer => writer.Write(helper.PageLink(x.Page)));
        }
    }
}