using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

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

		private static bool VisibleInMenu(IContent content)
		{
			var page = content as PageData;
			return page == null || page.VisibleInMenu;
		}
	}
}