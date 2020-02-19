using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.Common
{
    public static class TimeHelper
    {
        public const string DefaultDateFormat = "MM/dd/yyyy";
        public const string SQLServerDateFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DefaultTimeFormat = "hh:mmtt";
        public const string FullCalendarDateFormat = "yyyy-MM-dd";
        public static DateTime ParseExactDateTime(string calendar,string time)
        {
            string fullTime = time.Substring(0,time.Length-2);
            string amPm = time.Substring(time.Length - 2);
            string[] timeParts = fullTime.Split(new string[]{":"}, StringSplitOptions.None);
            if (timeParts[0].Length == 1)
            {
                timeParts[0] = "0" + timeParts[0];
            }
            if (timeParts[1].Length == 1)
            {
                timeParts[1] = "0" + timeParts[1];
            }
            fullTime = string.Format("{0}:{1}{2}", timeParts[0], timeParts[1], amPm);
            var exactTime = DateTime.ParseExact(
             string.Format("{0} {1}", calendar, fullTime),
             string.Format("{0} {1}", DefaultDateFormat, DefaultTimeFormat),
             CultureInfo.InvariantCulture);
            return exactTime;
        }
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public static void ConvertDateRangeStringToDateTime(string src,out DateTime start, out DateTime end)
        {
            if (string.IsNullOrEmpty(src))
            {
                start = DateTime.UtcNow;
                end = DateTime.UtcNow;
                return;
            }
            string[] seperators = new string[] { "-"}, splittedArr;
            splittedArr = src.Split(seperators, StringSplitOptions.None);
            if(splittedArr==null || splittedArr.Length!=2)
            {
                start = DateTime.UtcNow;
                end = DateTime.UtcNow;
                return;
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            start = DateTime.ParseExact(splittedArr[0].Trim(), DefaultDateFormat, provider);
            end = DateTime.ParseExact(splittedArr[1].Trim(), DefaultDateFormat, provider);
            return;
        }
    }
}
