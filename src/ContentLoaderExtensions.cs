using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    public static class ContentLoaderExtensions
    {
        /// <summary>
        /// Gets the first child of of the content item represented by the provided reference. 
        /// </summary>
        /// <param name="contentLoader"></param>
        /// <param name="contentReference"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFirstChild<T>(this IContentLoader contentLoader, ContentReference contentReference) where T : IContentData
        {
            return contentLoader.GetChildren<T>(contentReference).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first child of of the content item represented by the provided reference. 
        /// </summary>
        /// <param name="contentLoader"></param>
        /// <param name="contentReference"></param>
        /// <param name="culture"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFirstChild<T>(this IContentLoader contentLoader, ContentReference contentReference, CultureInfo culture) where T : IContentData
        {
            return contentLoader.GetChildren<T>(contentReference, culture).FirstOrDefault();
        }
    }
}