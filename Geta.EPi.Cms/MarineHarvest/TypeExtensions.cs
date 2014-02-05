using System;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class TypeExtensions
    {
        public static PageType GetPageType(this Type pageType)
        {
            return ServiceLocator.Current.GetInstance<PageTypeRepository>().Load(pageType);
        }
    }
}