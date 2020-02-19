using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MVCSite.Common
{
    public sealed class ArrayHelper
    {

        private ArrayHelper()
        { }

        /// <summary>
        /// string.Join的包装，如果array为null，defaultResult。
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="array"></param>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        public static string Join(string separator, string[] array, string defaultResult)
        {
            if (array == null) return defaultResult;
            else return string.Join(separator, array);
        }

        /// <summary>
        /// string.Join的包装，如果array为null，返回空字符串。
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string Join(string separator, string[] array)
        {
            return Join(separator, array, "");
        }

        /// <summary>
        /// string.Join的包装，如果array为null，返回空字符串；不为空，返回“,”分隔的字符串。
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string Join(string[] array)
        {
            if (array == null) return string.Empty;

            return Join(",", array);
        }

        public static string[] ToStringArray(ICollection collection)
        {
            string[] results = new string[collection.Count];
            collection.CopyTo(results, 0);
            return results;
        }

        public static string[] ToStringArray(char[] chars)
        {
            string[] result = new string[chars.Length];
            for (int i = 0; i < chars.Length; i++)
                result[i] = chars[i].ToString();
            return result;
        }

        /// <summary>
        /// 截取子Array，如果to超限，则截取到array尾部；如果from>to，则返回null。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string[] SubArray(string[] array, int from, int to)
        {
            if (from > to) return null;

            to = array.Length - 1 < to ? array.Length - 1 : to;
            string[] result = new string[to - from + 1];
            Array.Copy(array, from, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// 按照字符串长度进行排序，长的在上面，等长的保持原来的相对顺序。
        /// 使用冒泡排序，复杂度为：O(n2)，适用于小数组。
        /// </summary>
        /// <param name="array"></param>
        public static void SortByLength(string[] array)
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j].Length < array[i].Length)
                    {
                        string tmp = array[j];
                        array[j] = array[i];
                        array[i] = tmp;
                    }
                }
            }
        }

    }

}
