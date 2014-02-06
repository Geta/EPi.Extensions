using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EPi.Cms.Extensions
{
    public static class ContentReferenceExtensions
    {
        public static IEnumerable<PageData> GetChildren(this ContentReference contentLink)
        {
            return contentLink.GetChildren<PageData>();
        }

        public static IEnumerable<T> GetChildren<T>(this ContentReference contentLink) where T : IContentData
        {
            if (!contentLink.IsNullOrEmptyContentReference())
            {
                var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
                return repository.GetChildren<T>(contentLink);
            }

            return Enumerable.Empty<T>();
        }

        public static PageData GetPage(this ContentReference contentLink)
        {
            return contentLink.GetPage<PageData>();
        }

        public static T GetPage<T>(this ContentReference contentLink) where T : PageData
        {
            if (!contentLink.IsNullOrEmptyContentReference())
            {
                var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
                return loader.Get<PageData>(contentLink) as T;
            }

            return null;
        }

        public static string GetFriendlyUrl(this ContentReference contentLink, bool includeHost = false)
        {
            if (!contentLink.IsNullOrEmptyContentReference())
            {
                var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
                var url = urlResolver.GetUrl(contentLink);

                if (!includeHost)
                {
                    return url;
                }

                var urlBuilder = new UrlBuilder(url);

                if (HttpContext.Current != null)
                {
                    urlBuilder.Scheme = HttpContext.Current.Request.Url.Scheme;
                    urlBuilder.Host = HttpContext.Current.Request.Url.Host;
                    urlBuilder.Port = HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    urlBuilder.Scheme = SiteDefinition.Current.SiteUrl.Scheme;
                    urlBuilder.Host = SiteDefinition.Current.SiteUrl.Host;
                    urlBuilder.Port = SiteDefinition.Current.SiteUrl.Port;
                }

                return urlBuilder.ToString();
            }

            return string.Empty;
        }

        public static bool IsNullOrEmptyContentReference(this ContentReference contentLink)
        {
            return ContentReference.IsNullOrEmpty(contentLink);
        }
    }
}