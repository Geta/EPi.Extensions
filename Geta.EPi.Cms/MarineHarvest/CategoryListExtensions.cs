using System.Linq;
using EPiServer.Core;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class CategoryListExtensions
    {
        public static bool Contains(this CategoryList list, string name)
        {
            return list.Select(list.GetCategoryName).Any(categoryName => categoryName.Equals(name));
        }
    }
}
