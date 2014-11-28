using System;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Geta.EPi.Extensions.QueryString;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     UrlHelper extensions
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        ///     Returns the target URL for a PageReference. Respects the page's shortcut setting
        ///     so if the page is set as a shortcut to another page or an external URL that URL
        ///     will be returned.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="pageLink">Page reference for which to return URL.</param>
        /// <param name="defaultValue">Default value which will be returned if URL not found.</param>
        /// <returns>
        ///     Returns Html string with URL if URL found otherwise Html string with <paramref name="defaultValue" />
        /// </returns>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper, PageReference pageLink, string defaultValue)
        {
            var url = urlHelper.PageLinkUrl(pageLink) as MvcHtmlString;
            return MvcHtmlString.IsNullOrEmpty(url) ? new HtmlString(defaultValue) : url;
        }

        /// <summary>
        ///     Returns the target URL for a PageReference. Respects the page's shortcut setting
        ///     so if the page is set as a shortcut to another page or an external URL that URL
        ///     will be returned.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="pageLink">Page reference for which to return URL.</param>
        /// <returns>Returns Html string with URL.</returns>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper, PageReference pageLink)
        {
            if (ContentReference.IsNullOrEmpty(pageLink))
            {
                return MvcHtmlString.Empty;
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var page = contentLoader.Get<PageData>(pageLink);
            return urlHelper.PageUrl(page);
        }

        /// <summary>
        ///     Returns the target URL for a page. Respects the page's shortcut setting
        ///     so if the page is set as a shortcut to another page or an external URL that URL
        ///     will be returned.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="page">Page for which to find URL.</param>
        /// <returns>Returns Html string with URL.</returns>
        public static IHtmlString PageUrl(this UrlHelper urlHelper, PageData page)
        {
            switch (page.LinkType)
            {
                case PageShortcutType.Normal:
                case PageShortcutType.FetchData:
                    return urlHelper.PageUrl(page.LinkURL);

                case PageShortcutType.Shortcut:
                    var shortcutProperty = page.Property["PageShortcutLink"] as PropertyPageReference;
                    if (shortcutProperty != null && !ContentReference.IsNullOrEmpty(shortcutProperty.PageLink))
                    {
                        return urlHelper.PageLinkUrl(shortcutProperty.PageLink);
                    }
                    break;

                case PageShortcutType.External:
                    return new MvcHtmlString(page.LinkURL);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        ///     Creates QueryStringBuilder instance for provided EPiServer page.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="content">Content for which to create builder.</param>
        /// <returns>Instance of QueryStringBuilder for provided page.</returns>
        public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, IContent content)
        {
            if (content == null)
            {
                return QueryStringBuilder.Empty;
            }

            var url = urlHelper.ContentUrl(content.ContentLink);
            return QueryStringBuilder.Create(url);
        }

        /// <summary>
        ///     Creates QueryStringBuilder instance for provided EPiServer page.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="contentLink">ContentReference for which to create builder.</param>
        /// <returns>Instance of QueryStringBuilder for provided page.</returns>
        public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, ContentReference contentLink)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return QueryStringBuilder.Empty;
            }

            var url = urlHelper.ContentUrl(contentLink);
            return QueryStringBuilder.Create(url);
        }

        /// <summary>
        ///     Creates QueryStringBuilder instance for provided <paramref name="url" />.
        /// </summary>FilterSortOrder
        /// <param name="urlHelper">UrlHelper instance.</param>
        /// <param name="url">Url for which to create builder.</param>
        /// <returns>Instance of QueryStringBuilder for provided <paramref name="url" /></returns>
        public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, string url)
        {
            return QueryStringBuilder.Create(url);
        }

        /// <summary>
        ///     Appends action name to the end of a content url.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance</param>
        /// <param name="contentLink">ContentReference</param>
        /// <param name="actionName">The action name</param>
        public static string ContentActionUrl(this UrlHelper urlHelper, ContentReference contentLink, string actionName)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return string.Empty;
            }

            var contentUrl = urlHelper.ContentUrl(contentLink);

            if (string.IsNullOrWhiteSpace(actionName) || actionName.Equals("Index", StringComparison.OrdinalIgnoreCase))
            {
                return contentUrl;
            }

            var urlBuilder = new UrlBuilder(contentUrl);
            urlBuilder.Path = VirtualPathUtility.AppendTrailingSlash(urlBuilder.Path) + actionName;
            return urlBuilder.ToString();
        }

        /// <summary>
        ///     Appends action name to the end of a content url.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instance</param>
        /// <param name="content">IContent instance</param>
        /// <param name="actionName">The action name</param>
        public static string ContentActionUrl(this UrlHelper urlHelper, IContent content, string actionName)
        {
            if (content == null)
            {
                return string.Empty;
            }

            return ContentActionUrl(urlHelper, content.ContentLink, actionName);
        }
    }
}