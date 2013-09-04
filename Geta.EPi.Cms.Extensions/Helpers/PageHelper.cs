using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Cms.Helpers
{
	public static class PageHelper
	{
		/// <summary>
		/// Gets the start page untyped.
		/// </summary>
		/// <returns>PageData object</returns>
		public static PageData GetStartPage()
		{
			return GetStartPage<PageData>();
		}

		/// <summary>
		/// Gets the start page typed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>The typed start page</returns>
		public static T GetStartPage<T>() where T : PageData
		{
			var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
			return loader.Get<PageData>(ContentReference.StartPage) as T;
		}
	}
}