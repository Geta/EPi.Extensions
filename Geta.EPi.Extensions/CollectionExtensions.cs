using System.Collections.Generic;

namespace Geta.EPi.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Appends new item to the collection and returns instance of collection.
        /// </summary>
        /// <typeparam name="TCollection">Type of collection to modify.</typeparam>
        /// <typeparam name="TItem">Type of item to append.</typeparam>
        /// <param name="collection">Collection to modify.</param>
        /// <param name="item">Item to append.</param>
        /// <returns>Modified collection.</returns>
        public static TCollection Append<TCollection, TItem>(this TCollection collection, TItem item) 
            where TCollection : ICollection<TItem>
        {
            collection.Add(item);
            return collection;
        }
    }
}