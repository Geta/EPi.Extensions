using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Geta.EPi.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Filters the elements of an <see cref="T:System.Collections.IEnumerable"/> based on a specified type.
        /// Returns empty sequence if source sequence is null.
        /// NOTE: Helper extension to work with legacy APIs which might return null references of IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type to filter the elements of the sequence on.</typeparam>
        /// <param name="source">The <see cref="T:System.Collections.IEnumerable"/> whose elements to filter.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains elements from the input sequence of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> SafeOfType<T>(this IEnumerable source)
        {
	        return source == null ? Enumerable.Empty<T>() : source.OfType<T>().OrEmptyIfNull();
            
        }

        /// <summary>
        /// Returns empty sequence if source sequence is null otherwise returns source sequence.
        /// NOTE: Helper extension to work with legacy APIs which might return null references of IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements of the sequence.</typeparam>
        /// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1"/> to check.</param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains elements from the input sequence of type <typeparamref name="T"/>
        /// or empty sequence.
        /// </returns>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}