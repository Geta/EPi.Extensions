using System.Collections.Concurrent;
using EPiServer.Core;
using Geta.EPi.Extensions.SingletonPage;

namespace Geta.EPi.Extensions.Tests.SingletonPage
{
    public class FakeCache : DefaultContentReferenceCache
    {
        public ConcurrentDictionary<CacheKey, ContentReference> InternalCache => Cache;
    }
}