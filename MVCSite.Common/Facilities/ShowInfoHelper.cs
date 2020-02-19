using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

namespace MVCSite.Common
{
    public static class ShowInfoHelper
    {
        public static string[] LocationTagPostfix = new string[] { "省", "区", "县", "市", "州", "區", "岛", "城", "单位", "部", "路", "镇", "旗", "桥", "街", "沽" };
        public const int ByteConversion = 1024;
        public static double BytesOfGB = Math.Pow(ByteConversion, 3);
        public static double BytesOfMB = Math.Pow(ByteConversion, 2);

        public static string ToFileSize(this int source)
        {
            return ToFileSize(Convert.ToInt64(source));
        }

        public static string ToFileSize(this long source)
        {

            double bytes = Convert.ToDouble(source);

            if (bytes >= BytesOfGB) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(ByteConversion, 3), 2), " G");
            }
            else if (bytes >= BytesOfMB) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(ByteConversion, 2), 2), " M");
            }
            else if (bytes >= ByteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / ByteConversion, 2), " K");
            }
            else //Bytes
            {
                return string.Concat(bytes, " Bytes");
            }
        }
        public static string GetPrettyDate(this DateTime d,bool isEn=false)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.UtcNow.Subtract(d);
            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;
            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;
            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return d.ToString("yyyy-MM-dd");
            }
            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return isEn?"just now":"刚才";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return isEn?"1 minute ago":"1分钟前";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format(isEn?"{0} minutes ago":"{0}分钟前",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return isEn?"1 hour ago":"1小时前";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format(isEn?"{0} hours ago":"{0}小时前",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return isEn?"yesterday":"昨天";
            }
            if (dayDiff < 7)
            {
                return string.Format(isEn?"{0} days ago":"{0}天前",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format(isEn?"{0} weeks ago":"{0}周前",
                Math.Ceiling((double)dayDiff / 7));
            }
            return d.ToString("yyyy-MM-dd");
        }
        public static string GetPrettyDistance(this float d, bool isEn = false)
        {
            double result = Math.Round(d*10)/10;
            return string.Format("{0}{1}", result,isEn?"KM":"公里");
        }

        /// <summary>
        /// method to read in an enum and return a string array
        /// </summary>
        /// <param name="type">enum to convert</param>
        /// <returns></returns>
        public static Hashtable GetEnumTranslation(Type type, string[] translate)
        {
            //use Reflection to get the fields in our enum
            FieldInfo[] info = type.GetFields();

            //new ArrayList to hold the values (will convert later)
            Hashtable fields = new Hashtable();
            var i = 0;
            //loop through all the fields
            foreach (FieldInfo fInfo in info)
            {
                if (fInfo.FieldType.Name == type.Name)
                {
                    //add each to our ArrayList
                    fields.Add(fInfo.Name, translate[i++]);
                }
            }
            //now we convert to string array and return
            return fields;
        }


        public static string GetPrettyAddress(string address, int minLen = 3, int maxLen = 10) //
        {
            if (string.IsNullOrEmpty(address) || address.Length <= minLen)
                return address;
            Regex badDigitsRegex = new Regex(@"[a-zA-Z]*\d+$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            address = badDigitsRegex.Replace(address, "");
            var parts = address.Select((c, i) => address.Substring(i)).Where(x => x.Length >= minLen + 1)
                .Where(sub => LocationTagPostfix.Contains(sub.Substring(0, 1)) || LocationTagPostfix.Contains(sub.Substring(0, 2)));
            var lastOne = parts.LastOrDefault();
            string result = string.Empty;
            if (lastOne == null)
            {
                result = address;
            }
            else
            {
                if (LocationTagPostfix.Contains(lastOne.Substring(0, 2)))
                    result = lastOne.Substring(2);
                else
                    result = lastOne.Substring(1);
            }
            if (result.Length > maxLen)
            {
                result = result.Substring(0, maxLen - 3) + "...";
            }
            return result;
        }

    }
}
