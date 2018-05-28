using System;
using System.Collections.Generic;

namespace L2dotNET.Shared
{
    public static class Utilities
    {

        /// <summary>
        /// String extension method in order
        /// to check if two strings match no matter the case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// String extension method in order
        /// to check if two strings match with the exact the case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool EqualsMatchCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool StartsWithMatchCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCulture);
        }

        public static bool EndsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EndsWithMatchCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCulture);
        }

        public static SortedList<TKey, TValue> ToSortedList<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            SortedList<TKey, TValue> ret = new SortedList<TKey, TValue>();

            foreach (TSource item in source)
                ret.Add(keySelector(item), valueSelector(item));

            return ret;
        }
    }
}