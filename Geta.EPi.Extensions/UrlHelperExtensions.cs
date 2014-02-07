using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Geta.EPi.Extensions.QueryString;

namespace Geta.EPi.Cms.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Returns the target URL for a PageReference. Respects the page's shortcut setting
        /// so if the page is set as a shortcut to another page or an external URL that URL
        /// will be returned.
        /// </summary>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper, PageReference pageLink, string defaultValue)
        {
            var url = urlHelper.PageLinkUrl(pageLink) as MvcHtmlString;
            return MvcHtmlString.IsNullOrEmpty(url) ? new HtmlString(defaultValue) : url;
        }

        /// <summary>
        /// Returns the target URL for a PageReference. Respects the page's shortcut setting
        /// so if the page is set as a shortcut to another page or an external URL that URL
        /// will be returned.
        /// </summary>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper, PageReference pageLink)
        {
            if (!ContentReference.IsNullOrEmpty(pageLink))
            {
				var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
				var page = contentLoader.Get<PageData>(pageLink);
				return urlHelper.PageUrl(page);
            }

			return MvcHtmlString.Empty;
		}

        /// <summary>
        /// Returns the target URL for a page. Respects the page's shortcut setting
        /// so if the page is set as a shortcut to another page or an external URL that URL
        /// will be returned.
        /// </summary>
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

		public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, PageReference pageLink)
		{
			if (!PageReference.IsNullOrEmpty(pageLink))
			{
				var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
				var page = contentLoader.Get<PageData>(pageLink);
				return urlHelper.QueryBuilder(page);
			}

			return QueryStringBuilder.Empty;
		}

		public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, PageData page)
		{
			if (page != null)
			{
				var url = urlHelper.PageUrl(page);
				return QueryStringBuilder.Create(url.ToHtmlString());
			}

			return QueryStringBuilder.Empty;
		}

		public static QueryStringBuilder QueryBuilder(this UrlHelper urlHelper, string url)
		{
			return QueryStringBuilder.Create(url);
		}
	}
}