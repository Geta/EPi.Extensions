using System;
using EPiServer.Core;

namespace Geta.EPi.Extensions.SingletonPage
{
    public interface IContentReferenceCache
    {
        ContentReference GetOrAdd(CacheKey key, Func<ContentReference> valueFactory);
    }

    public class CacheKey
    {
        public Type Type { get; }
        public ContentReference ParentLink { get; }

        public CacheKey(Type type, ContentReference parentLink)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (parentLink == null) throw new ArgumentNullException(nameof(parentLink));
            Type = type;
            ParentLink = parentLink;
        }

        protected bool Equals(CacheKey other)
        {
            return Type == other.Type && Equals(ParentLink, other.ParentLink);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CacheKey) obj);
        }

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