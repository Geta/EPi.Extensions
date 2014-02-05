using System.Text.RegularExpressions;
using EPiServer.Configuration;

namespace Geta.EPi.Cms.Coop
{
    public static class ObjectExtensions
    {
        //public static string Translate(this object obj, string xpath)
        //{
        //    return EPiServer.Core.LanguageManager.Instance.Translate(xpath);
        //}

        /// <summary>
        /// Get the web string representation of the property's value, on properties that do not use the editor this string will usually not contain any markup characters.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The safe property value</returns>
        public static string ToWebString(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            string[] parsedUISafeHtmlTags = ParsedUISafeHtmlTags;
            string input = obj.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            if (((parsedUISafeHtmlTags != null) && (parsedUISafeHtmlTags.Length != 0)) && ((parsedUISafeHtmlTags.Length != 1) || (parsedUISafeHtmlTags[0].Length != 0)))
            {
                return Regex.Replace(input, BuildRegularExpression(parsedUISafeHtmlTags), "<$1>", RegexOptions.IgnoreCase);
            }

            return input;
        }

        private static string BuildRegularExpression(string[] safeTags)
        {
            return ("&lt;(/?(" + string.Join("|", safeTags) + @")/?\s*)&gt;");
        }

        private static string[] ParsedUISafeHtmlTags
        {
            get
            {
                string uISafeHtmlTags = Settings.Instance.UISafeHtmlTags;
                if (!string.IsNullOrEmpty(uISafeHtmlTags))
                {
                    return uISafeHtmlTags.Split(new[] { ',' });
                }
                return new string[0];
            }
        }
    }
}
