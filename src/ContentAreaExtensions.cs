using System.Linq;
using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// Content area extensions.
    /// </summary>
    public static class ContentAreaExtensions
    {
        /// <summary>
        /// Checks if content area has content.
        /// </summary>
        /// <param name="contentArea">The content area.</param>
        /// <returns>Returns true if content area has content and false when not.</returns>
        public static bool HasContent(this ContentArea contentArea)
        {
            return contentArea?.FilteredItems != null && contentArea.FilteredItems.Any();
        }
    }
}