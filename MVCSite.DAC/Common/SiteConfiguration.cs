using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using MVCSite.DAC.Interfaces;

namespace MVCSite.DAC.Common
{
    public class SiteConfiguration : ISiteConfiguration
    {
        public SiteConfiguration()
        {
            var folderPath = ConfigurationManager.AppSettings["UploadImageDir"] ?? @"C:\\";
            UploadImagesFolder = new DirectoryInfo(folderPath);
            UserName = new ValidationSettings
            {
                ValidationRegex = new Regex("^[a-zA-Z0-9]{4,50}$"),
                ValidationMessage = "用户名只能由字母和数字组成,四位以上"
            };
            Password = new ValidationSettings
            {
                ValidationRegex = new Regex("^.{8,}"),
                ValidationMessage = "At least 8 characters for password"
            };
            NickName = new ValidationSettings
            {
                ValidationRegex = new Regex("^.{4,15}"),
                ValidationMessage = "昵称为4-15个字符"
            };

            DontReplyEmailAddress = ConfigurationManager.AppSettings["DontReplyEmailAddress"] ?? string.Empty;
            AbuseReportEmailAddress = ConfigurationManager.AppSettings["AbuseReportEmailAddress"] ?? string.Empty;
            ContactUserEmailAddress = ConfigurationManager.AppSettings["ContactEmailAddress"] ?? string.Empty;
            ServerUrl = ConfigurationManager.AppSettings["ServerUrl"] ?? @"http://localhost:10284";
            if (ServerUrl.EndsWith(@"/"))
                ServerUrl = ServerUrl.Substring(0, ServerUrl.Length - 1);



            DefaultPageSize = 10;
        }

        public DirectoryInfo UploadImagesFolder { get; set; }
        public string DontReplyEmailAddress { get; set; }
        public string AbuseReportEmailAddress { get; set; }
        public string ContactUserEmailAddress { get; set; }
        public ValidationSettings UserName { get; set; }
        public ValidationSettings Password { get; set; }
        public ValidationSettings NickName { get; set; }

        public Uri MyDealBagServiceURL { get; set; }

        public int DefaultPageSize { get; set; }

        public string ServerUrl { get; set; }

        public bool JudgeCityByRequestUserIp { get; set; }

    }

    public class ValidationSettings
    {
        public Regex ValidationRegex { get; set; }
        public string ValidationMessage { get; set; }
    }

}
