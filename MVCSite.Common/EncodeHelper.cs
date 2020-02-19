using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCSite.Common
{
    public partial class EncodeHelper
    {
        //public const string keyStr = "C6HlgsnA3Bz2FLOPbcW7ZaXSYUeVdfhiKEjmopIJqrktDGuvxMNy0145w8QRT9+/=";
        public const string keyStr4Url = "C6HlgsnA3Bz2FLOPbcW7ZaXSYUeVdfhiKEjmopIJqrktDGuvxMNy0145w8QRT9_$=";
        public static string Encode4JavascriptStr(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(input);
            var base64 = Convert.ToBase64String(bytes);
            return base64.Replace("=", "$");
        }
        public static string Decode4JavascriptStr(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var data = Convert.FromBase64String(input.Replace("$", "="));
            var result = Encoding.UTF8.GetString(data);
            return result;
        }
        public static string Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var output = "";
            var chr1 = 0;
            var chr2 = 0;
            var chr3 = 0;
            var enc1 = 0;
            var enc2 = 0;
            var enc3 = 0;
            var enc4 = 0;
            var i = 0;
            var valueReg = new Regex(@"[^A-Za-z0-9\+\/\=]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            input = valueReg.Replace(input, "");
            do
            {
                enc1 = keyStr4Url.IndexOf(input[i++]);
                enc2 = keyStr4Url.IndexOf(input[i++]);
                enc3 = keyStr4Url.IndexOf(input[i++]);
                enc4 = keyStr4Url.IndexOf(input[i++]);
                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;
                output = output + (char)chr1;// string.Format("{0}", chr1);
                if (enc3 != 64)
                {
                    output = output + (char)chr2;// string.Format("{0}", chr2);
                }
                if (enc4 != 64)
                {
                    output = output + (char)chr3;// string.Format("{0}", chr3);
                }
                chr1 = chr2 = chr3 = 0;
                enc1 = enc2 = enc3 = enc4 = 0;
            } while (i < input.Length);
            //return HttpUtility.UrlDecode(output);
            return UTF8Decode(output);
            
        }
        public static string Encode(string input)
        {
            input = UTF8Encode(input);
            var output = "";
            int chr1, chr2, chr3;
            int enc1, enc2, enc3, enc4;
            int i = 0;
            int totalLen=input.Length;
            chr1 = chr2 = chr3 = 0;
            enc1 = enc2 = enc3 = enc4 = 0;
            do {
                chr1 = input[i++];
                if (i < totalLen)
                    chr2 = input[i++];
                if (i < totalLen)
                    chr3 = input[i++];
                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;
                if (chr2<=0) {
                    enc3 = enc4 = 64;
                } else if (chr3<=0) {
                    enc4 = 64;
                }
                output = output +
                    keyStr4Url[enc1] +
                    keyStr4Url[enc2] +
                    keyStr4Url[enc3] +
                    keyStr4Url[enc4];
                chr1 = chr2 = chr3 = 0;
                enc1 = enc2 = enc3 = enc4 = 0;
            } while (i < totalLen);
            return output;
        }
        public static string UTF8Encode(string input)
        {
            input = input.Replace("\r\n","\n");
            string utftext = "";
            for (var n = 0; n < input.Length; n++)
            {
	            var c = input[n];
 
	            if (c < 128) {
                    utftext += Convert.ToChar(c);// string.Format("{0}", c);
	            }
	            else if((c > 127) && (c < 2048)) {
                    utftext += Convert.ToChar((c >> 6) | 192);// string.Format("{0}", (c >> 6) | 192);
                    utftext += Convert.ToChar((c & 63) | 128);// string.Format("{0}", (c & 63) | 128);
	            }
	            else {
                    utftext += Convert.ToChar((c >> 12) | 224);// string.Format("{0}", (c >> 12) | 224);
                    utftext += Convert.ToChar(((c >> 6) & 63) | 128);// string.Format("{0}", ((c >> 6) & 63) | 128);
                    utftext += Convert.ToChar((c & 63) | 128);// string.Format("{0}", (c & 63) | 128);
	            }
 
            }
            return utftext;
        }
 

        public static string UTF8Decode(string utftext)
        {
            var result = "";
            int i = 0;
            int c, c1, c2, c3;
            c = c1 = c2 = c3 = 0;
 
            while ( i < utftext.Length ) {
 
	            c = utftext[i];
 
	            if (c < 128) {
                    result +=Convert.ToChar( c);// string.Format("{0}", c);
		            i++;
	            }
	            else if((c > 191) && (c < 224)) {
		            c2 = utftext[i+1];
                    result += Convert.ToChar(((c & 31) << 6) | (c2 & 63));// string.Format("{0}", ((c & 31) << 6) | (c2 & 63));
		            i += 2;
	            }
	            else {
		            c2 = utftext[i+1];
		            c3 = utftext[i+2];
                    result += Convert.ToChar(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));// string.Format("{0}", ((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
		            i += 3;
	            }
 
            }
 
            return result;
        }
        public static string CreateRandomPassword(int length)
        {
            const string valid = "!@#$%&*abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random(DateTime.UtcNow.Millisecond);
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
