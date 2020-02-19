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
        /// string.Join�İ�װ�����arrayΪnull��defaultResult��
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
        /// string.Join�İ�װ�����arrayΪnull�����ؿ��ַ�����
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string Join(string separator, string[] array)
        {
            return Join(separator, array, "");
        }

        /// <summary>
        /// string.Join�İ�װ�����arrayΪnull�����ؿ��ַ�������Ϊ�գ����ء�,���ָ����ַ�����
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
        /// ��ȡ��Array�����to���ޣ����ȡ��arrayβ�������from>to���򷵻�null��
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
        /// �����ַ������Ƚ������򣬳��������棬�ȳ��ı���ԭ�������˳��
        /// ʹ��ð�����򣬸��Ӷ�Ϊ��O(n2)��������С���顣
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
