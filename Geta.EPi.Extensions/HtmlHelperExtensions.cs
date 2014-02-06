using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Geta.EPi.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Create(this HtmlHelper helper, string element, string content)
        {
            return Create(helper, element, () => true, () => new MvcHtmlString(content));
        }

        public static MvcHtmlString Create(this HtmlHelper helper, string element, Func<bool> shouldWrap, string content)
        {
            return Create(helper, element, shouldWrap, () => new MvcHtmlString(content));
        }

        public static MvcHtmlString Create(this HtmlHelper helper, string element, Func<bool> shouldWrap, Func<MvcHtmlString> content)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            if (shouldWrap())
            {
                var tagBuilder = new TagBuilder(element.ToLower()) { InnerHtml = content().ToHtmlString() };
                writer.Write(tagBuilder.ToString(TagRenderMode.Normal));
            }

            return new MvcHtmlString(buffer.ToString());
        }

        public static MvcHtmlString WrapActionLink(this HtmlHelper helper, bool shouldWrapFunc, string link, string text)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            if (!shouldWrapFunc)
            {
                writer.Write(text);
            }
            else
            {
                var tagBuilder = new TagBuilder("a") { InnerHtml = text };
                tagBuilder.MergeAttribute("href", link);
                writer.Write(tagBuilder.ToString(TagRenderMode.Normal));
            }

            return new MvcHtmlString(buffer.ToString());
        }
    }
}
