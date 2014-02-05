using System;
using System.Collections.Generic;
using System.Web.Routing;
using EPiServer.Core;

namespace Geta.EPi.Cms.Coop
{
    public static class HtmlImageExtensions
    {
        public static string HtmlImage(this PageData page, string imagePropertyName)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(page.PropertyValue<string>(imagePropertyName), string.Empty, null);
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, string imagePropertyName, object attributes)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(page.PropertyValue<string>(imagePropertyName), string.Empty, new RouteValueDictionary(attributes));
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, string imagePropertyName, string altTextPropertyName)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(page.PropertyValue<string>(imagePropertyName), page.PropertyValue<string>(altTextPropertyName), null);
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, string imagePropertyName, string altTextPropertyName, object attributes)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(page.PropertyValue<string>(imagePropertyName), page.PropertyValue<string>(altTextPropertyName), new RouteValueDictionary(attributes));
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath, Func<PageData, string> altText)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), altText(page), null);
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath, Func<PageData, string> altText, object attributes)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), altText(page), new RouteValueDictionary(attributes));
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath, string altText)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), altText, null);
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath, string altText, object attributes)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), altText, new RouteValueDictionary(attributes));
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), string.Empty, null);
            }

            return string.Empty;
        }

        public static string HtmlImage(this PageData page, Func<PageData, string> imagePath, object attributes)
        {
            if (page.IsEPiServerPage())
            {
                return HtmlImage(imagePath(page), string.Empty, new RouteValueDictionary(attributes));
            }

            return string.Empty;
        }

        public static string HtmlImage(string imagePath, string altText, IDictionary<string, object> attributes)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return string.Empty;
            }

            return HtmlHelper.GenerateImage(imagePath, altText, attributes);
        }
    }
}
