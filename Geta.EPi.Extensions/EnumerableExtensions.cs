using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Geta.EPi.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> SafeOfType<TResult>(this IEnumerable source)
        {
	        return source == null ? Enumerable.Empty<TResult>() : source.OfType<TResult>();
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}