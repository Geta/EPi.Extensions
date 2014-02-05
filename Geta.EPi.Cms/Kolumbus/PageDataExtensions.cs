using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Cms.Kolumbus
{
    public static class PageDataExtensions
    {
        public static int ChildCount<T>(this PageData page) where T : IContentData
        {
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return loader.GetChildren<T>(page.ContentLink).Count();
        }
    }
}