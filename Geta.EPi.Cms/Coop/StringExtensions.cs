using EPiServer.Core.Html;

namespace Geta.EPi.Cms.Coop
{
    public static class StringExtensions
    {
        /// <summary>
        ///     CurrentPage.MainIntro.FormatHtml("<p>", "</p>");
        /// </summary>
        /// <param name="text">Text value</param>
        /// <param name="startTag">The start tag</param>
        /// <param name="endTag">The closing end tag</param>
        /// <returns>Formatted string with the property value between the start and end tag</returns>
        public static string FormatHtml(this string text, string startTag, string endTag)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return string.Format("{0}{1}{2}", startTag, text, endTag);
        }

        public static string StripHtml(this string htmlText, int maxLength = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return htmlText;
            }

            return TextIndexer.StripHtml(htmlText, maxLength);
        }
    }
}
