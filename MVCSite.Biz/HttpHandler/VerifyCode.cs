
//***************************************************************
// 文件名:		VerifyCode.cs
// 版权:		Copyright @ 2007 WMath
// 创建人:		Robert
// 代码实现:	
// 修改人:		
// 描述:		验证码
// 备注:		
//***************************************************************
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic;
using WMath.Facilities;

namespace MVCSite.Biz.HttpHandler
{
    public static class VerifyCode
	{
        // 在全部修改完成前为了不与以前的验证码实现相冲突,暂用一个不同的CacheKey
        private const string VerifyKey = "_VerifyKey_";
        private const int MaxCount = 100000;

        public static string CreateVerifyCode()
        {
            Dictionary<string, VerifyCodeItem> dict = HttpContext.Current.Session[VerifyKey] as Dictionary<string, VerifyCodeItem>;
            if (dict == null)
            {
                dict = new Dictionary<string, VerifyCodeItem>(StringComparer.OrdinalIgnoreCase);
                HttpContext.Current.Session[VerifyKey] = dict;
            }

            // 移除过时的验证码
            if (dict.Count >= MaxCount) RemoveExpiredCode(dict);

            VerifyCodeItem item = new VerifyCodeItem();
            item.CodeID = Guid.NewGuid().ToString();
            item.Code = new Random().Next(0, 10000).ToString("0000");
            item.AddTime = DateTime.Now;
            dict.Add(item.CodeID, item);

            return item.CodeID;
        }

        public static string GetVerifyCode(string codeID)
        {
            VerifyCodeItem item;
            Dictionary<string, VerifyCodeItem> dict = HttpContext.Current.Session[VerifyKey] as Dictionary<string, VerifyCodeItem>;
            if (dict == null) return string.Empty;
            else if (!dict.TryGetValue(codeID, out item)) return string.Empty;

            return item.Code;
        }

        public static void RemoveVerifyCode(string codeID)
        {
            Dictionary<string, VerifyCodeItem> dict = HttpContext.Current.Session[VerifyKey] as Dictionary<string, VerifyCodeItem>;
            if (dict != null) dict.Remove(codeID);
        }

        /// <summary>
        /// 验证输入的注册码是否正确
        /// </summary>
        public static bool Validate(string codeID, string code, ref string errHtml)
        {
            string errorStr = string.Empty;
            bool isValid = true;
            if (string.IsNullOrEmpty(code))
            {
                errorStr = "请输入验证码";
                isValid = false;
            }
            else
            {
                string vc = GetVerifyCode(codeID);
                if (string.IsNullOrEmpty(vc))
                {
                    errorStr = "对不起，验证码已失效,请刷新页面";
                    isValid = false;
                }
                else if (vc != code)
                {
                    errorStr = "验证码输入有误,请重新填写";
                    isValid = false;
                }
            }

            if (errHtml != string.Empty) errHtml += "<br>";
            errHtml += errorStr;
            return isValid;
        }

        /// <summary>
        /// 获取验证码图片URL
        /// </summary>
        /// <param name="code">验证码codeID</param>
        /// <returns></returns>
        public static string GetImageUrl(string code)
        {
            //string codeEncrypt = HttpUtility.UrlEncode(DataProtection.Encrypt(code, DataProtection.Store.User));
			string codeEncrypt = VCode.GetVcode( code );
            return string.Format("/ValidateCode.ashx?code={0}", codeEncrypt);
        }

        private static void RemoveExpiredCode(Dictionary<string, VerifyCodeItem> dict)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, VerifyCodeItem> kvp in dict)
            {
                if (kvp.Value.AddTime.AddHours(2) < DateTime.Now) list.Add(kvp.Key);
            }

            foreach (string codeID in list)
            {
                dict.Remove(codeID);
            }
        }

        internal struct VerifyCodeItem
        {
            public string CodeID;
            public string Code;
            public DateTime AddTime;
        }

	}
}
