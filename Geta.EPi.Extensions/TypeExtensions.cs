using System;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Type extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Returns PageType for provided Type.
        /// </summary>
        /// <param name="pageType">Type for which to lookup PageType.</param>
        /// <returns>PageType instance if found.</returns>
        public static PageType GetPageType(this Type pageType)
        {
            return ServiceLocator.Current.GetInstance<PageTypeRepository>().Load(pageType);
        }
    }
}