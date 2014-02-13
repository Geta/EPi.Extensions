using System.Collections.Generic;
using System.Linq;
using EPiServer.HtmlParsing;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    ///     Helper methods for working with EPiServer XForms.
    /// </summary>
    public static class XFormHelper
    {
        /// <summary>
        ///     Replaces XForm HTML tables with more semantic markup.
        /// </summary>
        /// <param name="htmlFragment">HtmlFragment of XForm</param>
        /// <param name="addClassAttribute">
        ///     Will add individual HTML class for each element.
        ///     For matching HTML classes see <see cref="XFormHelper.TableTagCssClasses" />
        /// </param>
        /// <returns>Modified HtmlFragment</returns>
        public static HtmlFragment CleanupXFormHtmlMarkup(HtmlFragment htmlFragment, bool addClassAttribute = true)
        {
            var originalTag = htmlFragment.Name;

            var elFragment = htmlFragment as ElementFragment;
            if (elFragment == null || !IsTableFragment(elFragment))
            {
                return htmlFragment;
            }

            elFragment.Name = "div";
            RemoveAttribute(elFragment, "valign");

            if (addClassAttribute)
            {
                AddClassAttribute(originalTag, elFragment);
            }

            return elFragment;
        }

        /// <summary>
        ///     Dictionary of TABLE tags and matching HTML classes for markup replacements
        ///     when cleaning XForm using <see cref="XFormHelper.CleanupXFormHtmlMarkup" />
        ///     method with parameter addClassAttribute = true
        /// </summary>
        public static Dictionary<string, string> TableTagCssClasses = new Dictionary<string, string>
        {
            {"table", "xform-table"},
            {"tr", "xform-row"},
            {"td", "xform-col"},
            {"tbody", "xform-body"},
            {"thead", "xform-thead"}
        };

        private static void AddClassAttribute(string originalTag, ElementFragment elFragment)
        {
            var classAttribute = GetCssClass(originalTag);

            if (classAttribute != null)
                elFragment.Attributes.Add(classAttribute);
        }

        private static void RemoveAttribute(ElementFragment elFragment, string attribute)
        {
            if (elFragment.HasAttributes && elFragment.Attributes[attribute] != null)
            {
                elFragment.Attributes.Remove(attribute);
            }
        }

        private static bool IsTableFragment(ElementFragment elementFragment)
        {
            var tableTags = new[] {"table", "tr", "td", "thead", "tbody"};
            return tableTags.Contains(elementFragment.Name);
        }

        private static AttributeFragment GetCssClass(string tag)
        {
            return new AttributeFragment
            {
                Name = "class",
                Value = TableTagCssClasses[tag]
            };
        }
    }
}