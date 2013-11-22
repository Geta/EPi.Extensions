using EPiServer.Core.Html;

namespace Geta.EPi.Cms.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtml(string htmlText, int maxLength = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return htmlText;
            }

            return TextIndexer.StripHtml(htmlText, maxLength);
        }
    }
}