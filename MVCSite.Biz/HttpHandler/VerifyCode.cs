
//***************************************************************
// �ļ���:		VerifyCode.cs
// ��Ȩ:		Copyright @ 2007 WMath
// ������:		Robert
// ����ʵ��:	
// �޸���:		
// ����:		��֤��
// ��ע:		
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
        // ��ȫ���޸����ǰΪ�˲�����ǰ����֤��ʵ�����ͻ,����һ����ͬ��CacheKey
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

            // �Ƴ���ʱ����֤��
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
        /// ��֤�����ע�����Ƿ���ȷ
        /// </summary>
        public static bool Validate(string codeID, string code, ref string errHtml)
        {
            string errorStr = string.Empty;
            bool isValid = true;
            if (string.IsNullOrEmpty(code))
            {
                errorStr = "��������֤��";
                isValid = false;
            }
            else
            {
                string vc = GetVerifyCode(codeID);
                if (string.IsNullOrEmpty(vc))
                {
                    errorStr = "�Բ�����֤����ʧЧ,��ˢ��ҳ��";
                    isValid = false;
                }
                else if (vc != code)
                {
                    errorStr = "��֤����������,��������д";
                    isValid = false;
                }
            }

            if (errHtml != string.Empty) errHtml += "<br>";
            errHtml += errorStr;
            return isValid;
        }

        /// <summary>
        /// ��ȡ��֤��ͼƬURL
        /// </summary>
        /// <param name="code">��֤��codeID</param>
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
