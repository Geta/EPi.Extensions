using EPiServer.HtmlParsing;

namespace Geta.EPi.Cms.Helpers
{
	/// <summary>
	/// Helper methods for working with EPiServer XForms.
	/// </summary>
    public static class XFormHelper
    {
		/// <summary>
		/// Replaces tables with divs.
		/// </summary>
		/// <param name="htmlFragment"></param>
		/// <param name="addClassAttribute"></param>
		/// <returns>Modified HtmlFragment</returns>
        public static HtmlFragment CleanupXFormHtmlMarkup(HtmlFragment htmlFragment, bool addClassAttribute = true)
        {
			var originalTag = htmlFragment.Name;
	        var isTableRow = originalTag == "tr";

            if (originalTag == "table" || isTableRow || originalTag == "td" || originalTag == "tbody" || originalTag == "thead")
            {
                htmlFragment.Name = "div";

	            var elFragment = htmlFragment as ElementFragment;

	            if (elFragment != null)
	            {
		            if (elFragment.HasAttributes && elFragment.Attributes["valign"] != null)
		            {
			            elFragment.Attributes.Remove("valign");
		            }

		            if (addClassAttribute)
		            {
			            var classAttribute = GetCssClass(originalTag);

			            if (classAttribute != null)
				            elFragment.Attributes.Add(classAttribute);
		            }
	            }
            }

            return htmlFragment;
        }

	    private static AttributeFragment GetCssClass(string originalTag)
	    {
		    var attr = new AttributeFragment()
			    {
				    Name = "class"
			    };

		    switch (originalTag)
		    {
			    case "table":
				    attr.Value = "xform-table";
				    break;
				case "tr":
				    attr.Value = "xform-row";
				    break;
				case "td":
				    attr.Value = "xform-col";
				    break;
				case "tbody":
				    attr.Value = "xform-body";
				    break;
				case "thead":
					attr.Value = "xform-thead";
					break;
				default:
				    return null;
		    }

		    return attr;
	    }
    }
}