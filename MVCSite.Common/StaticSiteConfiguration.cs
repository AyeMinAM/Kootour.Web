using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using MVCSite.Common;

namespace MVCSite.DAC.Common
{
    public class StaticSiteConfiguration
    {
        public StaticSiteConfiguration()
        {
    
        }
        public static readonly int DefaultMinutesCountAsNowCall = 5;
        public static readonly int DefaultActivityCount = 500;
        public static readonly int DefaultBoardListWidth = 260;//px
        public static string ImageServerUrl = ConfigurationManager.AppSettings["ImageServerUrl"] ?? "http://img.vjiaoshi.com/";
        //public static string KootourServerUrl = ConfigurationManager.AppSettings["KootourServerUrl"] ?? "http://go.kootour.com/";
        //public static string KootourGuiderUrl = ConfigurationManager.AppSettings["KootourGuiderUrl"] ?? "http://guides.kootour.com/";
        //public static string KootourUrl = ConfigurationManager.AppSettings["KootourGuiderUrl"] ?? "http://www.kootour.com/";

        public static string CrawlerServiceUrl = ConfigurationManager.AppSettings["CrawlerServiceUrl"] ?? "http://localhost:58067/";
        public static string SongSiteHost = ConfigurationManager.AppSettings["SongSiteHost"] ?? "tanchang8.com";
        public static bool IsOfflineTest = ConfigurationManager.AppSettings["IsOfflineTest"] == null ? false : SafeConvert.ToBoolean(ConfigurationManager.AppSettings["IsOfflineTest"]);
        public static string ChangBaURL = ConfigurationManager.AppSettings["ChangBaURL"] ?? "http://api.changba.com/ktvboxmp.php?ac=searchSongsByKeyword&num=10&type=-1&first=1&start=0&keyword={0}&bless=1&channelsrc=changba&macaddress=18%3Adc%3A56%3A88%3A12%3A33&deviceid=18%3Adc%3A56%3A88%3A12%3A33&version=6.5.1&seret={1}&_userinfo={2}";

        public static string ImageFileDirectory = ConfigurationManager.AppSettings["ImageFileDirectory"] ?? @"D:\JImage\";
        public static string MediaDirectory = ConfigurationManager.AppSettings["MediaDirectory"] ?? @"D:\JImage\Media\";
        public static string TTSServiceDirectory = ConfigurationManager.AppSettings["TTSServiceDirectory"] ?? @"D:\JImage\TTSService\";
        public static string CrawlSpecificIds = ConfigurationManager.AppSettings["CrawlSpecificIds"] ?? @"";
        public static string CrawlSpecificWords = ConfigurationManager.AppSettings["CrawlSpecificWords"] ?? @"";
        public static int TranslateStartId = ConfigurationManager.AppSettings["TranslateStartId"]==null ? 1
            : SafeConvert.ToInt32(ConfigurationManager.AppSettings["TranslateStartId"]);
        public static int TranslateEndId = ConfigurationManager.AppSettings["TranslateEndId"] == null ? 10
            : SafeConvert.ToInt32(ConfigurationManager.AppSettings["TranslateEndId"]);
        
        
        
        public static Regex NoBlankSpace4MoreStringRegex = new Regex(@"^\w{4,}");
        public static readonly int WelcomeBoardTemplateID = 1;
        public static readonly int InitialBoardTemplateID = 1;

        public static string GoogleConsumerKey = "anonymous";
        public static string GoogleConsumerSecret = "anonymous";

        public static bool UnitTestBeforePublish = true ;
        public static int CategoryXorSeed = 0xFFCDEF;
        public static bool EnableLogPrintOut = ConfigurationManager.AppSettings["EnableLogPrintOut"] ==null? true:SafeConvert.ToBoolean(ConfigurationManager.AppSettings["EnableLogPrintOut"]);
        public static int SleepSecondsAfterCrawlPage = ConfigurationManager.AppSettings["SleepSecondsAfterCrawlPage"] == null ? 5 : SafeConvert.ToInt32(ConfigurationManager.AppSettings["SleepSecondsAfterCrawlPage"]);
        //public static bool IsGuiderSite = ConfigurationManager.AppSettings["IsGuiderSite"] == null ? false : SafeConvert.ToBoolean(ConfigurationManager.AppSettings["IsGuiderSite"]);
        public static string CSREmailAddress = ConfigurationManager.AppSettings["CSREmailAddress"] == null ? "contact@kootour.com" :
            ConfigurationManager.AppSettings["CSREmailAddress"];

        public static string UserIdInCookie = ConfigurationManager.AppSettings["UserIdInCookie"] == null ? "eed1d30c67923caf27cff30ceede2fe8" : ConfigurationManager.AppSettings["UserIdInCookie"];
        public static string UserNameInCookie = ConfigurationManager.AppSettings["UserNameInCookie"] == null ? "superlwj@hotmail.com" : ConfigurationManager.AppSettings["UserNameInCookie"];
        public static string UserPasswordInCookie = ConfigurationManager.AppSettings["UserPasswordInCookie"] == null ? "1abef07781699bc008a89980275cfa01" : ConfigurationManager.AppSettings["UserPasswordInCookie"];
        public static string BlackWords = ConfigurationManager.AppSettings["BlackWords"] == null ? "toWords" : ConfigurationManager.AppSettings["BlackWords"];

        public static string MobileVersionCode = ConfigurationManager.AppSettings["MobileVersionCode"] == null ? "1" :
            ConfigurationManager.AppSettings["MobileVersionCode"];
        public static string MobileVersionSummary = ConfigurationManager.AppSettings["MobileVersionSummary"] == null ? "MobileVersionSummary" :
            ConfigurationManager.AppSettings["MobileVersionSummary"];
        public static string MobileVersion = ConfigurationManager.AppSettings["MobileVersion"] == null ? "MobileVersion" :
            ConfigurationManager.AppSettings["MobileVersion"];
        public static string MobileVersionForceUpgradeBefore = ConfigurationManager.AppSettings["MobileVersionForceUpgradeBefore"] == null ? "0" :
            ConfigurationManager.AppSettings["MobileVersionForceUpgradeBefore"];
        public static int[] DefaultRecommendedUserIds = ConfigurationManager.AppSettings["DefaultRecommendedUserIds"] == null ?null :
            ConfigurationManager.AppSettings["DefaultRecommendedUserIds"].SplitToIntArray(";");
        public static string ClientJSVersion = ConfigurationManager.AppSettings["ClientJSVersion"] == null ? "1" :
            ConfigurationManager.AppSettings["ClientJSVersion"];
        public static int DefaultTryUserID = ConfigurationManager.AppSettings["DefaultTryUserID"] == null ? 1 :
            SafeConvert.ToInt32(ConfigurationManager.AppSettings["DefaultTryUserID"]);
        public static int WordPicDictBookID = ConfigurationManager.AppSettings["WordPicDictBookID"] == null ? 347 :
            SafeConvert.ToInt32(ConfigurationManager.AppSettings["WordPicDictBookID"]);

        public static int CrawlWordStartBookID = ConfigurationManager.AppSettings["CrawlWordStartBookID"] == null ? 0 : SafeConvert.ToInt32(ConfigurationManager.AppSettings["CrawlWordStartBookID"]);
        public static int CrawlingContentType = ConfigurationManager.AppSettings["CrawlingContentType"] == null ? 1 : SafeConvert.ToInt32(ConfigurationManager.AppSettings["CrawlingContentType"]);

        public static string GetXorCtrInfo(int lft, int rgt, int id)
        {
            return string.Format("{0}-{1}-{2}", StaticSiteConfiguration.CategoryXorSeed ^ lft,
                StaticSiteConfiguration.CategoryXorSeed ^ rgt, StaticSiteConfiguration.CategoryXorSeed ^ id);
        }       
    }
}