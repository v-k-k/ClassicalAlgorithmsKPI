using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public static class Extensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static void UnpackTo<T1, T2>(this Tuple<T1, T2> t, out T1 v1, out T2 v2)
        {
            v1 = t.Item1;
            v2 = t.Item2;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static T[][] SkipIfContains<T>(this T[][] source, T[] ignoreValues)
        {
            HashSet<T> uniqueIgnore = new HashSet<T>(ignoreValues);
            List<int> indexes = new List<int>(); 
            for (int i = 0; i < source.Length; i++)
            {
                if (new HashSet<T>(source[i]).SetEquals(uniqueIgnore))
                    indexes.Add(i);
            }

            // Return new array.
            T[][] res = new T[source.Length - indexes.Count][];
            int checkIdx = 0;
            int sourceIdx = 0;
            for (int i = 0; i < res.Length; i++)
            {
                checkIdx = i;
                while (indexes.Contains(checkIdx))
                {
                    sourceIdx++;
                    checkIdx++;
                }
                res[i] = source[sourceIdx];
                sourceIdx++;
            }
            
            return res;
        }

        public static T[] Append<T>(this T[] source, T item)
        {
            Array.Resize(ref source, source.Length + 1);
            source[source.Length - 1] = item;
            return source;
        }

        public static TSource MinBy<TSource>(
            this IEnumerable<TSource> source, Func<TSource, IComparable> projectionToComparable)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("Sequence is empty.");
                }
                TSource min = e.Current;
                IComparable minProjection = projectionToComparable(e.Current);
                while (e.MoveNext())
                {
                    IComparable currentProjection = projectionToComparable(e.Current);
                    if (currentProjection.CompareTo(minProjection) < 0)
                    {
                        min = e.Current;
                        minProjection = currentProjection;
                    }
                }
                return min;
            }
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
    }
}
