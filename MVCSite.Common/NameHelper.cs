using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.Common.NameHelper
{
    public static class NameHelper
    {

        public static string GenDriveName(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return string.Empty;

            return originalName
                .Replace("'", "")
                .Replace("/", "")
                .Replace(" ", "-");
        }
        public static string GetCityName(string originalName)
        {
            if (!originalName.Contains(','))
                return originalName;

            return originalName.Split(',')[0];
        }

        //public static string GetDashedCityName(string originalName)
        //{
        //    if (string.IsNullOrEmpty(originalName))
        //        return originalName;
        //    //string result = GetCityName(originalName);
        //    return originalName.Replace(" ", "-");
        //}
        public static string SpaceToDash(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return originalName;

            return originalName.Replace(" ", "-");
        }
        public static string DashToSpace(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return originalName;

            return originalName.Replace("-", " ");
        }
        public static string GenTourInUrl(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return originalName;
            var result = originalName.Replace(" ", "-");
            result = result.Replace("/", "");
            result = Regex.Replace(result, @"[,:&%""]+", "-");
            result = Regex.Replace(result, @"[-]{2,}", "-");
            if (result.StartsWith("-"))
                result = result.Substring(1);
            if (result.EndsWith("-"))
                result = result.Substring(0,result.Length-1);
            return result.Trim();
        }
        public static bool CompareSpacedGeoNames(string name1, string name2)
        {
            name1 = DashToSpace(name1).ToLower();
            name2 = DashToSpace(name2).ToLower();
            return (string.Compare(name1, name2) == 0);
        }
        public static bool CompareDashedGeoNames(string name1, string name2)
        {
            name1 = SpaceToDash(name1).ToLower();
            name2 = SpaceToDash(name2).ToLower();
            return (string.Compare(name1, name2) == 0);
        }
        public static string GetFirstParagraph(string original)
        {
            var openTag = original.IndexOf(@"<p>");
            var closeTag = original.IndexOf(@"</p>");
            if (openTag == 0 && closeTag > openTag)
            {
                string sub = original.Substring(openTag + 3, closeTag - openTag - 3);
                return Regex.Replace(sub, @"<.*?>|&.*?;", string.Empty);
            }
            return Regex.Replace(original, @"<.*?>|&.*?;", string.Empty);
        }
        public static string GetCityNameFromUniqueCityNameInUrl(string originalName)
        {
            if (!originalName.Contains('-'))
                return originalName;

            return originalName.Split('-')[0];
        }
    }
}
