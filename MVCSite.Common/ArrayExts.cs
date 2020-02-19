using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace MVCSite.Common
{
    public static class ArrayExts
    {
        private const string TAG = "ArrayExts";
        public static bool Contains<T>(this T[] data, T val)
        {
            if (data == null || data.Length <= 0)
                return false;
            for (int i = 0, length = data.Length; i < length; i++)
            {
                if (data[i].Equals(val))
                    return true;
            }
            return false;
        }
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static int[] SplitToIntArray(this string str, string seperator)
        {
            return str.SplitToIntArray(new string[] { seperator });
        }
        public static int[] SplitToIntArray(this string str, string[] seperators)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            string[] maxIds = str.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            List<int> intArr = new List<int>();
            for (int i = 0, length = maxIds.Length; i < length; i++)
            {
                intArr.Add(SafeConvert.ToInt32(maxIds[i]));
            }
            return intArr.ToArray();
        }
        public static string GetAggregateString<T>(this IEnumerable<T> models, string seperator)
        {
            if (models == null || string.IsNullOrEmpty(seperator))
                return string.Empty;
            var enumerator = models.GetEnumerator();
            StringBuilder sb = new StringBuilder(256);
            while (enumerator.MoveNext())
            {
                sb.AppendFormat("{0}{1}", enumerator.Current.ToString(), seperator);
            }
            var result = sb.ToString();
            if (string.IsNullOrEmpty(result))
                return "";
            if (result.Length - seperator.Length <= 0)
                return "";
            return result.Substring(0, result.Length - seperator.Length);
        }
    }

}