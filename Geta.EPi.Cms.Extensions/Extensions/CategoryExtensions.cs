using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;

namespace Geta.EPi.Cms.Extensions
{
    public static class CategoryExtensions
    {
         public static IEnumerable<Category> GetCategories(this int categoryRootId)
         {
             return Category.Find(categoryRootId).Categories
                                     .Cast<Category>();
         }
    }
}