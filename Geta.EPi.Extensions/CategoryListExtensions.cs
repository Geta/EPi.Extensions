using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace Geta.EPi.Extensions
{
    public static class CategoryListExtensions
    {
        /// <summary>
        /// Returns string of comma separated category LocalizedDescription.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <param name="requireAvailable">Mark if return only categories with Available = true.</param>
        /// <param name="requireSelectable">Mark if return only categories with Selectable = true.</param>
        /// <returns>String of comma separated category LocalizedDescription.</returns>
        public static string GetCommaSeparatedCategories(this CategoryList categoryList,
            bool requireAvailable = false,
            bool requireSelectable = false)
        {
            var categories = categoryList.GetFullCategories();

            if (requireAvailable)
            {
                categories = categories.Where(x => x.Available);
            }

            if (requireSelectable)
            {
                categories = categories.Where(x => x.Selectable);
            }

            var categoryDescriptions = categories.OrderBy(c => c.LocalizedDescription)
                .Select(c => c.LocalizedDescription);

            return string.Join(", ", categoryDescriptions);
        }

        /// <summary>
        /// Returns enumeration of Category instances for provided CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <returns>Enumeration of Category instances.</returns>
        public static IEnumerable<Category> GetFullCategories(this CategoryList categoryList)
        {
            return categoryList != null
                ? categoryList.Select(Category.Find).Where(category => category != null)
                : Enumerable.Empty<Category>();
        }

        /// <summary>
        /// Returns names of categories for provided CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <returns>Enumeration of category names.</returns>
        public static IEnumerable<string> GetCategoryNames(this CategoryList categoryList)
        {
            return categoryList != null
                ? categoryList.Select(categoryList.GetCategoryName)
                : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Returns a value indicating whether the specified category by name occurs within this CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <param name="name">Category name to seek.</param>
        /// <returns></returns>
        public static bool Contains(this CategoryList categoryList, string name)
        {
            return categoryList.Select(categoryList.GetCategoryName).Any(categoryName => categoryName.Equals(name));
        }
    }
}