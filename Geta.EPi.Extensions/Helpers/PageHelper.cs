using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    /// Utility class to help work with EPiServer PageData objects
    /// </summary>
	public static class PageHelper
	{
		/// <summary>
		/// Gets the start page for current site.
		/// </summary>
		/// <returns>PageData object</returns>
		public static PageData GetStartPage()
		{
			return GetStartPage<PageData>();
		}

		/// <summary>
		/// Gets the start page of concrete type for current site.
		/// </summary>
		/// <typeparam name="T">StartPage type</typeparam>
		/// <returns>StartPage of <typeparamref name="T"/></returns>
		public static T GetStartPage<T>() where T : PageData
		{
			var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
			return loader.Get<PageData>(ContentReference.StartPage) as T;
		}
	}
}