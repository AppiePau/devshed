namespace Devshed.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// LINQ helper extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// A foreach helper function to iterate through a collection and execute an action per element.
        /// </summary>
        /// <typeparam name="T"> The type of items in the collection. </typeparam>
        /// <param name="collection"> The collection to iterate through. </param>
        /// <param name="action"> The action to perform. </param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action.Invoke(item);
            }

            return collection;
        }

        /// <summary>
        /// Selects (iterates through) a collection while counting items.
        /// </summary>
        /// <typeparam name="TSource"> The type of elements in the source collection. </typeparam>
        /// <typeparam name="TResult"> The type of elements in the result collection to return. </typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectWithCount<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            var items = source.ToArray();

            for (int index = 0; index < items.Length; index++)
            {
                var item = items[index];
                int itemCount = index + 1;

                yield return selector(item, itemCount);
            }
        }

        /// <summary>
        /// Removes items form a collection using a selector / filter.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        public static void RemoveAll<TSource>(this ICollection<TSource> source, Func<TSource, bool> selector)
        {
            var items = source.Where(selector).ToArray();

            foreach (var item in items)
            {
                source.Remove(item);
            }
        }

        /// <summary>
        /// Adds an array based items to a collection.
        /// </summary>
        /// <typeparam name="TSource"> Type of source items. </typeparam>
        /// <param name="source"> The source list. </param>
        /// <param name="items"> The items to add. </param>
        public static void AddRange<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// Remove an item from a collection.
        /// </summary>
        /// <typeparam name="TSource"> Type of source items. </typeparam>
        /// <param name="source"> The source list. </param>
        /// <param name="items"> The items to remove. </param>
        public static void Remove<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Remove(item);
            }
        }
    }
}
