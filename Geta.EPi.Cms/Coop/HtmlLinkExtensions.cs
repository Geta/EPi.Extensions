using System;
using System.Web.Routing;
using EPiServer.Core;

namespace Geta.EPi.Cms.Coop
{
    public static class HtmlLinkExtensions
    {
        public static string HtmlLink(this PageData page)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, page.PageName, null);
        }

        public static string HtmlLink(this PageData page, string linkText)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, linkText, null);
        }

        public static string HtmlLink(this PageData page, Func<PageData, string> linkText)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, linkText(page), null);
        }

        public static string HtmlLink(this PageData page, object attributes)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, page.PageName, new RouteValueDictionary(attributes));
        }

        public static string HtmlLink(this PageData page, string linkText, object attributes)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, linkText, new RouteValueDictionary(attributes));
        }

        public static string HtmlLink(this PageData page, Func<PageData, string> linkText, object attributes)
        {
            if (!page.IsEPiServerPage())
            {
                return string.Empty;
            }

            return HtmlLink(page, linkText(page), new RouteValueDictionary(attributes));
        }
    }
}

