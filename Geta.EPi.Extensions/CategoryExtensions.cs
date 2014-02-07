using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Category extensions.
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        ///     Returns child categories of provided parent category ID
        /// </summary>
        /// <param name="categoryRootId">Parent category ID</param>
        /// <returns>Enumeration of child categories</returns>
        public static IEnumerable<Category> GetChildCategories(this int categoryRootId)
        {
            var root = Category.Find(categoryRootId);
            return root != null
                ? root.Categories.Cast<Category>()
                : Enumerable.Empty<Category>();
        }
    }
}