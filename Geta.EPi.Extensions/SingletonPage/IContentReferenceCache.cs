using System;
using EPiServer.Core;

namespace Geta.EPi.Extensions.SingletonPage
{
    /// <summary>
    /// A singleton page's content reference cache.
    /// </summary>
    public interface IContentReferenceCache
    {
        /// <summary>
        /// Gets or adds singleton page's content reference to the cache.
        /// </summary>
        /// <param name="key">A cache key as a pair of a root page's content reference and a singleton page's type.</param>
        /// <param name="valueFactory">A factory function to get a singleton page's content reference.</param>
        /// <returns>A singleton page's content reference.</returns>
        ContentReference GetOrAdd(CacheKey key, Func<ContentReference> valueFactory);
    }

    /// <summary>
    /// A cache key as a pair of a root page's content reference and a singleton page's type.
    /// </summary>
    public class CacheKey
    {
        /// <summary>
        /// Singleton page's type.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// A root page's content reference under which singleton page gets searched.
        /// </summary>
        public ContentReference ParentLink { get; }

        /// <summary>
        /// Instantiates new CacheKey.
        /// </summary>
        /// <param name="type">Singleton page's type.</param>
        /// <param name="parentLink">A root page's content reference under which singleton page gets searched.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CacheKey(Type type, ContentReference parentLink)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (parentLink == null) throw new ArgumentNullException(nameof(parentLink));
            Type = type;
            ParentLink = parentLink;
        }

        /// <summary>
        /// Determines whether two specified CacheKey objects have the same value.
        /// </summary>
        /// <param name="other">The CacheKey to compare to this instance.</param>
        /// <returns>true if the specified CacheKey is equal to the current CacheKey; otherwise, false.</returns>
        protected bool Equals(CacheKey other)
        {
            return Type == other.Type && Equals(ParentLink, other.ParentLink);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CacheKey) obj);
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type?.GetHashCode() ?? 0)*397) 
                    ^ (ParentLink?.GetHashCode() ?? 0);
            }
        }
    }
}