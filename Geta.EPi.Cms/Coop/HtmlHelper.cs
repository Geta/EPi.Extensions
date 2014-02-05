using System;
using System.Collections.Generic;

namespace Geta.EPi.Cms.Coop
{
    public static class HtmlHelper
    {
        private static string _idAttributeDotReplacement;

        public static string IdAttributeDotReplacement
        {
            get
            {
                if (String.IsNullOrEmpty(_idAttributeDotReplacement))
                {
                    _idAttributeDotReplacement = "_";
                }
                return _idAttributeDotReplacement;
            }
            set
            {
                _idAttributeDotReplacement = value;
            }
        }

        public static string GenerateLink(string url, string linkText, IDictionary<string, object> attributes)
        {
            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!string.IsNullOrEmpty(linkText)) ? linkText.ToWebString() : string.Empty
            };

            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeAttribute("href", url);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        public static string GenerateImage(string imagePath, string altText, IDictionary<string, object> attributes)
        {
            var htmlImage = new TagBuilder("img");

            htmlImage.MergeAttributes(attributes);
            htmlImage.MergeAttribute("src", imagePath);
            htmlImage.MergeAttribute("alt", altText);

            return htmlImage.ToString(TagRenderMode.SelfClosing);
        }
    }
}
