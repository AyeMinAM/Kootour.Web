using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace MVCSite.Common
{
    public static class StringExtensions
    {
        private static byte[] key = new byte[8] { 5, 0, 0, 9, 4, 6, 7, 7 };
        private static byte[] iv = new byte[8] { 5, 0, 5, 0, 9, 4, 5, 0 };
        public static string Encrypt(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string result = Encoding.Unicode.GetString(outputBuffer);
            return result;
        }

        public static string GetFirstN(this string source, int length)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            if (source.Length <= length)
                return source;
            return source.Substring(0, length);
        }
        public static string EnsureLessThan(this string source,int length)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            if (source.Length <= length)
                return source;
            if (length <= 3)
                return source.Substring(0, length);
            return source.Substring(0, length - 3) + "...";
        }
        public static bool IsPhoneNumber(this string source)
        {
            return Regex.Match(source, @"^([0-9]{5,15})$").Success;
                //||Regex.Match(source, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$").Success;
        }
        public static string CleanHtmlTag(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            var tagRegex = new Regex(@"\<[^\>]+\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return tagRegex.Replace(source, "");
        }
        public static string CleanHtmlBlankText(this string source)
        {
            return source.Replace("\t", "").Replace("\r", "").Replace("\n", "")
                .Replace("&nbsp;", "").Replace("&gt;", ">").Replace("&lt;", "<")
                .Replace("&raquo;", "»").Replace("&laquo;", "«").Replace(" ", "");
        }
        public static string ConvertXpathToRegex(this string source)
        {
            return source.Replace("[", @"\[").Replace("]", @"\]");
        }
        public static string ConvertRegexToXpath(this string source)
        {
            return source.Replace(@"\[", @"[").Replace(@"\]", @"]");
        }
        public static string StemCityNameCn(this string source)
        {
            return source.Replace(@"特别行政区", "").Replace(@"自治州", "").Replace(@"州", "").Replace("地区", "").Replace("自治区", "").Replace("区", "")
                .Replace("市", "").Replace("自治县", "").Replace("县", "").Replace("桥", "").Replace("镇", "").Replace("其他", "").Replace("其它", "");
        }
        public static string RemoveContentsInBracket(this string source)
        {
            var numberRegex = new Regex(@"[【｛（\[\{\(]{1}.*[】｝）\]\}\)]{1}$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return numberRegex.Replace(source,"");
        }    
        public static string RemoveLastSlashThenLowerTrim(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            if (input.EndsWith(@"/") || input.EndsWith(@"\"))
                return input.Substring(0, input.Length - 1).ToLower().Trim();
            else
                return input.ToLower().Trim();
        }
        public static string RemoveAllNonwordCharacters(this string source)
        {
            var numberRegex = new Regex(@"\W+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return numberRegex.Replace(source, "");
        } 
        public static bool IsWebsiteHostLink(this string link)
        {
            if (string.IsNullOrEmpty(link))
                return false;
            Uri uri;
            if (!Uri.TryCreate(link, UriKind.Absolute, out uri))
                return false;
            if (uri.PathAndQuery != @"/")
                return false;
            return true;
        }
        public static bool ContainsChinese(this string src)
        {
            // 匹配中文字符          
            var regex = new Regex("[\u4e00-\u9fa5]+");
            if (regex.IsMatch(src))
            {
                return true;
            }
            return false;
        }

    }
}