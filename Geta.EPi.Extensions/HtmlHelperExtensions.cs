using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     HtmlHelper extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        ///     Creates HTML element with text content.
        /// </summary>
        /// <param name="helper">HtmlHelper instance.</param>
        /// <param name="element">Element to create.</param>
        /// <param name="content">Content to add to element.</param>
        /// <returns>MvcHtmlString with HTML element and content.</returns>
        public static MvcHtmlString Create(this HtmlHelper helper, string element, string content)
        {
            return Create(helper, element, () => true, () => new MvcHtmlString(content));
        }

        /// <summary>
        ///     Creates HTML element with text content when <paramref name="shouldWrap" /> function returns true.
        /// </summary>
        /// <param name="helper">HtmlHelper instance.</param>
        /// <param name="element">Element to create.</param>
        /// <param name="shouldWrap">Function which indicates if has to create element.</param>
        /// <param name="content">Content to add to element.</param>
        /// <returns>
        ///     MvcHtmlString with HTML element and content if <paramref name="shouldWrap" />function returns true otherwise
        ///     returns empty MvcHtmlString.
        /// </returns>
        public static MvcHtmlString Create(this HtmlHelper helper, string element, Func<bool> shouldWrap, string content)
        {
            return Create(helper, element, shouldWrap, () => new MvcHtmlString(content));
        }

        /// <summary>
        ///     Creates HTML element with MvcHtmlString content when <paramref name="shouldWrap" /> function returns true.
        /// </summary>
        /// <param name="helper">HtmlHelper instance.</param>
        /// <param name="element">Element to create.</param>
        /// <param name="shouldWrap">Function which indicates if has to create element.</param>
        /// <param name="content">Function which returns MvcHtmlString content to add to element.</param>
        /// <returns>
        ///     MvcHtmlString with HTML element and content if <paramref name="shouldWrap" />function returns true otherwise
        ///     returns empty MvcHtmlString.
        /// </returns>
        public static MvcHtmlString Create(this HtmlHelper helper, string element, Func<bool> shouldWrap,
            Func<MvcHtmlString> content)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            if (shouldWrap())
            {
                var tagBuilder = new TagBuilder(element.ToLower()) {InnerHtml = content().ToHtmlString()};
                writer.Write(tagBuilder.ToString(TagRenderMode.Normal));
            }

            return new MvcHtmlString(buffer.ToString());
        }

        /// <summary>
        ///     Creates A tag with link if <paramref name="shouldWrap" /> is true otherwise outputs <paramref name="content" />
        /// </summary>
        /// <param name="helper">HtmlHelper instance.</param>
        /// <param name="shouldWrap">Mark which indicates if create A tag around content.</param>
        /// <param name="link">String representation of URL for which to create A tag.</param>
        /// <param name="content">Text which will be rendered.</param>
        /// <returns>
        ///     MvcHtmlString with A tag with <paramref name="content" /> and <paramref name="link" /> if
        ///     <paramref name="shouldWrap" /> is true otherwise returns MvcHtmlString with <paramref name="content" />.
        /// </returns>
        public static MvcHtmlString WrapActionLink(this HtmlHelper helper, bool shouldWrap, string link, string content)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            if (shouldWrap)
            {
                var tagBuilder = new TagBuilder("a") {InnerHtml = content};
                tagBuilder.MergeAttribute("href", link);
                writer.Write(tagBuilder.ToString(TagRenderMode.Normal));
            }
            else
            {
                writer.Write(content);
            }

            return new MvcHtmlString(buffer.ToString());
        }
    }
}