namespace Devshed.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action.Invoke(item);
            }

            return collection;
        }

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

        public static void RemoveAll<TSource>(this ICollection<TSource> source, Func<TSource, bool> selector)
        {
            var items = source.Where(selector).ToArray();

            foreach (var item in items)
            {
                source.Remove(item);
            }
        }

        public static void AddRange<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        public static void Remove<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Remove(item);
            }
        }
    }
}
