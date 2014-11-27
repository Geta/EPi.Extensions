using EPiServer.Core;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    ///     Helper methods for working with EPiServer PageData objects
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        ///     Gets the start page for current site.
        /// </summary>
        /// <returns>PageData object</returns>
        public static PageData GetStartPage()
        {
            return GetStartPage<PageData>();
        }

        /// <summary>
        ///     Gets the start page of concrete type for current site.
        /// </summary>
        /// <typeparam name="T">StartPage type</typeparam>
        /// <returns>StartPage of <typeparamref name="T" /></returns>
        public static T GetStartPage<T>() where T : PageData
        {
            return ContentReference.StartPage.Get<T>();
        }
    }
}