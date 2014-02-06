using EPiServer.Core;
using Geta.EPi.Extensions;

namespace Geta.EPi.Cms.Extensions
{
    public static class PageReferenceExtensions
    {
		public static string GetLinkUrl(this PageReference pageReference)
		{
			var page = pageReference.GetPage();
			return page != null ? page.LinkURL : string.Empty;
		}

	    public static bool IsNullOrEmptyPageReference(this PageReference pageReference)
        {
            return PageReference.IsNullOrEmpty(pageReference);
        }
    }
}