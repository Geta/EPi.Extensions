using System.Linq;
using EPiServer;
using EPiServer.Core;

namespace Geta.EPi.Extensions
{
    public static class ContentRepositoryExtensions
    {
        /// <summary>
        /// Gets the first child of of the content item represented by the provided reference. 
        /// </summary>
        /// <param name="contentRepository"></param>
        /// <param name="contentReference"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFirstChild<T>(this IContentRepository contentRepository, ContentReference contentReference) where T : IContentData
        {
            return contentRepository.GetChildren<T>(contentReference).FirstOrDefault();
        }
    }
}