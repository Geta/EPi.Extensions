using System;
using System.Collections.Concurrent;
using EPiServer.Core;

namespace Geta.EPi.Extensions.SingletonPage
{
    /// <summary>
    /// Default cache for singleton page content references.
    /// Uses ConcurrentDictionary.
    /// </summary>
    public class DefaultContentReferenceCache : IContentReferenceCache
    {
        /// <summary>
        /// Internal cache.
        /// </summary>
        protected ConcurrentDictionary<CacheKey, ContentReference> Cache { get; }

        /// <summary>
        /// Instantiates new DefaultContentReferenceCache.
        /// </summary>
        public DefaultContentReferenceCache()
        {
            Cache = new ConcurrentDictionary<CacheKey, ContentReference>();
        }

        /// <summary>
        /// Gets or adds a singleton page content reference to the cache when not empty.
        /// </summary>
        /// <param name="key">Cache key as a pair of the root page content reference and singleton page type.</param>
        /// <param name="valueFactory">Factory function to get singleton page content reference.</param>
        /// <returns>A singleton page's content reference.</returns>
        public ContentReference GetOrAdd(CacheKey key, Func<ContentReference> valueFactory)
        {
            ContentReference value;
            if (Cache.TryGetValue(key, out value))
            {
                return value;
            }
            value = valueFactory();
            return value == ContentReference.EmptyReference ? value : Cache.GetOrAdd(key, value);
        }
    }
}