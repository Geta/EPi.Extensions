using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.HtmlParsing;
using EPiServer.Web.Mvc.Html;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    ///     Helper methods for working with EPiServer XForms.
    /// </summary>
    public static class XFormHelper
    {
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
            {"tbody", "xform-body-col-{0}"},
            {"thead", "xform-thead"}
        };

        /// <summary>
        /// Gets number of table columns from html fragment collection.
        /// </summary>
        /// <param name="htmlFragments"></param>
        /// <returns></returns>
        public static int GetNumberOfTableColumns(IEnumerable<HtmlFragment> htmlFragments)
        {
            var fragmentsList = htmlFragments.ToList();
            var firstTrIndex = fragmentsList.FindIndex(x => x.Name == "tr");
            var tdCount = 0;

            if (firstTrIndex > -1)
            {
                for (int index = firstTrIndex + 1; index < fragmentsList.Count; index++)
                {
                    var fragment = fragmentsList[index];

                    if (fragment.Name == "tr")
                    {
                        break;
                    }

                    if (fragment.Name == "td" && fragment.FragmentType == HtmlFragmentType.Element)
                    {
                        tdCount++;
                    }
                }
            }

            if (tdCount == 0)
                tdCount++;

            return tdCount;
        }

        /// <summary>
        /// Renders clean XForm HTML markup. Replaces tables with divs.
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="htmlFragments">IEnumerable&lt;HtmlFragment&gt;</param>
        public static void RenderCleanXFormHtmlMarkup(this HtmlHelper helper, IEnumerable<HtmlFragment> htmlFragments)
        {
            int numTableColumns = GetNumberOfTableColumns(htmlFragments);

            foreach (var htmlFragment in htmlFragments)
            {
                helper.ViewContext.Writer.Write(helper.Fragment(CleanupXFormHtmlMarkup(htmlFragment, true, numTableColumns)));
            }
        }

        /// <summary>
        ///     Replaces XForm HTML tables with more semantic markup.
        /// </summary>
        /// <param name="htmlFragment">HtmlFragment of XForm</param>
        /// <param name="addClassAttribute">
        ///     Will add individual HTML class for each element.
        ///     For matching HTML classes see <see cref="XFormHelper.TableTagCssClasses" />
        /// </param>
        /// <param name="numTableColumns">Number of columns in the XForm table. Use GetNumberOfTableColumns to retrieve the value.</param>
        /// <returns>Modified HtmlFragment</returns>
        public static HtmlFragment CleanupXFormHtmlMarkup(HtmlFragment htmlFragment, bool addClassAttribute = true, int numTableColumns = 1)
        {
            var originalTag = htmlFragment.Name;

            if (!IsTableFragment(htmlFragment))
            {
                return htmlFragment;
            }

            htmlFragment.Name = "div";
            var elFragment = htmlFragment as ElementFragment;

            if (elFragment != null)
            {
                RemoveAttribute(elFragment, "valign");

                if (addClassAttribute)
                {
                    AddClassAttribute(originalTag, elFragment, numTableColumns);
                }
            }

            return htmlFragment;
        }

        private static void AddClassAttribute(string originalTag, ElementFragment elFragment, int numTableColumns)
        {
            var classAttribute = GetCssClass(originalTag, numTableColumns);

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

        private static bool IsTableFragment(HtmlFragment htmlFragment)
        {
            var tableTags = new[] {"table", "tr", "td", "thead", "tfoot", "tbody"};
            return tableTags.Contains(htmlFragment.Name);
        }

        private static AttributeFragment GetCssClass(string tag, int numTableColumns)
        {
            string cssClass = TableTagCssClasses[tag];

            if (tag == "tbody")
            {
                cssClass = numTableColumns == 1 ? "xform-body" : string.Format(cssClass, numTableColumns);
            }

            return new AttributeFragment
            {
                Name = "class",
                Value = cssClass
            };
        }
    }
}