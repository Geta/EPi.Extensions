using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Geta.EPi.Cms.Extensions
{
	public static class ContentExtensions
	{
		/// <summary>
		/// Filters content which should not be visible to the user.
		/// </summary>
		public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> content, bool requirePageTemplate = false, bool requireVisibleInMenu = false) where T : IContent
		{
            if (content != null)
            {
				var accessFilter = new FilterAccess();
				var publishedFilter = new FilterPublished(ServiceLocator.Current.GetInstance<IContentRepository>());
				content = content.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));

				if (requirePageTemplate)
				{
					var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
					templateFilter.TemplateTypeCategories = TemplateTypeCategories.Page;
					content = content.Where(x => !templateFilter.ShouldFilter(x));
				}

				if (requireVisibleInMenu)
				{
					content = content.Where(x => VisibleInMenu(x));
				}
				return content;
            }

			return Enumerable.Empty<T>();
		}

		public static PageData GetPage(this ContentReference contentLink)
		{
			return contentLink.GetPage<PageData>();
		}

		public static T GetPage<T>(this ContentReference contentLink) where T: PageData
		{
			var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
			return loader.Get<PageData>(contentLink) as T;
		}

		public static IEnumerable<PageData> GetChildren(this ContentReference contentLink)
		{
			return contentLink.GetChildren<PageData>();
		}

		public static IEnumerable<T> GetChildren<T>(this ContentReference contentLink) where T: IContentData
		{
			if (!contentLink.IsNullOrEmptyContentReference())
			{
				var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
				return repository.GetChildren<T>(contentLink);
			}

			return Enumerable.Empty<T>();
		}

		public static string GetFriendlyUrl(this ContentReference contentLink, bool includeHost = false)
		{
			if (!contentLink.IsNullOrEmptyContentReference())
			{
				var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
				var url = urlResolver.GetVirtualPath(contentLink);

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
					urlBuilder.Scheme = Settings.Instance.SiteUrl.Scheme;
					urlBuilder.Host = Settings.Instance.SiteUrl.Host;
					urlBuilder.Port = Settings.Instance.SiteUrl.Port;
				}

				return urlBuilder.ToString();
			}

			return string.Empty;
		}

		public static bool IsNullOrEmptyContentReference(this ContentReference contentLink)
		{
			return ContentReference.IsNullOrEmpty(contentLink);
		}

		private static bool VisibleInMenu(IContent content)
		{
			var page = content as PageData;
			return page == null || page.VisibleInMenu;
		}
	}
}