using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// PageReference extensions.
    /// </summary>
    public static class PageReferenceExtensions
    {
        /// <summary>
        /// Returns string representation of page LinkURL property for provided PageReference.
        /// </summary>
        /// <param name="pageReference"></param>
        /// <returns></returns>
		public static string GetLinkUrl(this PageReference pageReference)
		{
			var page = pageReference.GetPage();
			return page != null ? page.LinkURL : string.Empty;
		}

        /// <summary>
        /// Indicates whether the specified page reference is null or an EmptyReference.
        /// </summary>
        /// <param name="pageReference">Page reference to test.</param>
        /// <returns>true if page reference is null or EmptyReference else false</returns>
	    public static bool IsNullOrEmpty(this PageReference pageReference)
        {
            return PageReference.IsNullOrEmpty(pageReference);
        }
    }
}