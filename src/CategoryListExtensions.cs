using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     CategoryList extensions.
    /// </summary>
    public static class CategoryListExtensions
    {
        #pragma warning disable 649
        private static Injected<CategoryRepository> _categoryRepository;
        #pragma warning restore 649

        /// <summary>
        ///     Builds a comma separated string of categories using LocalizedDescription as name.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <param name="requireAvailable">Flag to filter by Available.</param>
        /// <param name="requireSelectable">Flag to filter by Selectable.</param>
        /// <returns>A comma separated string of categories using LocalizedDescription as name.</returns>
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
        ///     Returns enumeration of Category for provided CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <returns>Enumeration of Category.</returns>
        public static IEnumerable<Category> GetFullCategories(this CategoryList categoryList)
        {
            return categoryList != null
                ? categoryList.Select(_categoryRepository.Service.Get).Where(category => category != null)
                : Enumerable.Empty<Category>();
        }

        /// <summary>
        ///     Returns names of categories for provided CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <returns>Enumeration of category names.</returns>
        public static IEnumerable<string> GetCategoryNames(this CategoryList categoryList)
        {
            return categoryList != null
                ? categoryList.Select(c => _categoryRepository.Service.Get(c).Name)
                : Enumerable.Empty<string>();
        }

        /// <summary>
        ///     Returns a value indicating whether the specified category by name occurs within this CategoryList.
        /// </summary>
        /// <param name="categoryList">CategoryList with categories.</param>
        /// <param name="name">Category name to seek.</param>
        /// <returns>true if category exists in CategoryList otherwise false.</returns>
        public static bool Contains(this CategoryList categoryList, string name)
        {
            return categoryList.Select(c => _categoryRepository.Service.Get(c).Name).Any(categoryName => categoryName.Equals(name));
        }
    }
}