/*
 * This code is generated from JAVA.
*/

//using Com.Vjiaoshi;
//using Com.Vjiaoshi.DB;
//using Sharpen;
#define TanChang8

using System;
namespace MVCSite.Common
{
	public partial class ConstantData
	{
		public const string TAG = "ConstantData";

        public const int PageSizeToMobile = 5;        
        public static DateTime DefaultInitialDateTime = DateTime.UtcNow.AddYears(-5);
        public const string CommonTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string PatternBlank = "___";

        //public const int SampleRate = 8000;
        //public const int SampleRate = 16000;
        public static int SampleRate = 44100;
        public const int BitsPerSample = 16;
        public const int ChannelCount = 1;
        public const string RecordedWavFile = "Talk.wav";
        public const string RecordedMp3File = "Talk.mp3";

        public const int MaxCharCountInOneRow = 60;
        
        public const int MaxWordUnitToReserve4UnpaidUser = 50;
        public const int MaxWordUnitToReserveMediaInLocalFile = 30;
        //public const int MaxWordUnitToReserveMediaInLocalFile = 0;

        public const string RootPathUserAvatar = "UserAvt";
        public const string RootPathChatImage = "ChatImg";

        public const int FreeWordUnitToReserve = 3;

        public const int MoneyPoint4BonusEasy = 1;
        public const int MoneyPoint4BonusHard = 3;
        public const int MoneyPoint4LogonEveryday = 50;
        public const int MoneyPoint4ForwardWeiboOnce = 20;
        public const int MoneyPoint4BlackHoleShield = 100;
        public const int MoneyPoint4DownloadADictUnit = 1000;

        public const bool IsMediaWWWSolution = true;
        public const string SQLSelectInStringSeparator = ",";
        public const string DataPartSeparator = "_";
        public const string DataEmptyLyric = "-";
        public static string[] DataStringSeparators = new string[] { ",", "，" };
        public static string[] DataSectionSeparators = new string[] { ";", "；" };
		public const  string PLAY_USER_LOGON_ID = "__vjiaoshi_com__";

		public const  string PLAY_USER_PASSWORD = "_1_2_3_4_5_6_!@#$_";

		public const  string COOKIE_NAME = "_VJiaoshiCookie_";

		public const string ACTION_RESULT_KEY = "Result";

		public const string ACTION_RESULT_DATA_KEY = "HashMap";

		public const string ACTION_RESULT_SERVICE_CODE_KEY = "ServiceCode";

		public const string ACTION_RESULT_YES = "OK";

		public const string ACTION_RESULT_NO = "FAILED";

		public const string ACTION_RESULT_SUBMITTED = "SUBMITTED";

		public const string ACTION_RESULT_LOGON_TIMEOUT = "LOGON_TIMEOUT";

		public const string ACTION_RESULT_PROGRESS_STATUS = "STATUS";

		public const string FOLLOW_ACTION_KEY = "FOLLOW_ACTION";

		public const string FOLLOW_ACTION_REFRESH_MEDIA_DATA = "REFRESH_MEDIA_DATA";

		public const string LOCAL_RESULT_FILE_NOT_FOUND = "FILE_NOT_FOUND";


        public const string DATA_KEY_USER_NAME = "UserName";
        public const string DATA_KEY_USER_EMAIL = "Email";
        public const string DATA_KEY_PASSWORD = "Password";
        public const string DATA_KEY_NICKNAME = "NickName";
        public const string DATA_KEY_FROM_CLIENT = "From";
        public const string DATA_KEY_SEND_CONFIRM_EMAIL = "SendConfirmEmail";
        public const string DATA_KEY_RECOGNIZE_RESULT = "RECOGNIZE_RESULT";
        public const string DATA_KEY_RESULT_MESSAGE = "Message";
        public const string DATA_KEY_USER_DEVICE_ID = "DeviceID";

        public  const string _RenrenStatePrefix = "_RR_";
        public  const string _SinaStatePrefix = "_SN_";

        public const int BookIdOnSetup = 0;

        public const int MAX_COUNT_USE_RECOGNIZE_RESULT = 10;

		public const long TIMEOUT_SESSION = 1800;

		public const long TIMEOUT_ONE_PROGRESS = 90;

		public const long TIMEOUT_STAY_IN_FRONT_IDLE = 1;

		public static int RELOAD_NEW_WORDS_LESS_THAN = 100;
        public static int DOWNLOAD_MEDIA_LESS_THAN = 100;

		public const int COUNT_PER_GROUP4OFFLINE = 50;

		public const int MAX_TIMES_TRY_SPEAK_WORD = 3;

		public const int MAX_USER_CONTACT_CSR_INPUT_LEN = 1024;

		public const int MIN_USER_CONTACT_CSR_INPUT_LEN = 6;

		public const int REQUEST_CODE_RECOGNIZE_DIALOG = 1;

		public const int REQUEST_CODE_LOGON = 2;

		public const int REQUEST_CODE_SELECT_BOOKTYPE = 3;

		public const int REQUEST_CODE_SELECT_WORD_CHINESE = 4;

		public const int REQUEST_CODE_WORD_CHINESE_AND_USAGE = 5;

		public const int REQUEST_CODE_REGISTER = 6;

		public const int REQUEST_CODE_MY_SETTINGS = 7;

		public const int REQUEST_CODE_BOOK_SUMMARY = 8;

		public const int REQUEST_CODE_NOTIFICATION = 9;

		public const int REQUEST_CODE_CONTACT_US = 10;

		public const int REQUEST_CODE_ABOUT_US = 11;

		public const int REQUEST_CODE_PRONOUNCE_PRACTICE = 12;

		public const int REQUEST_CODE_HELP = 13;

		public const int REQUEST_CODE_SPEAK_IMAGE_WORD = 14;

		public const int REQUEST_CODE_SPEAK_SENTENCE_WORD = 15;

		public const int REQUEST_CODE_LISTEN_OUT_CHINESE = 16;

		public const int REQUEST_CODE_SELECT_IMAGE_CHINESE = 17;

		public const int REQUEST_CODE_SELECT_WORD_ENGLISH = 18;
        public const int REQUEST_CODE_SELECT_BOOK_IN_TYPE = 19;

		public static string[] optionPrefixes = new string[] { "A. ", "B. ", "C. "
			, "D. " };
        public static string[] englishOrdinalNoPostfixes = new string[] { " Ⅰ", " Ⅱ", " Ⅲ"
			, " Ⅳ" };
        public static string[] chineseOrdinalNoPostfixes = new string[] { "(一)", "(二)", "(三)"
			, "(四)" };
		public const string SoundMarkFormat = "[ {0} ]";


		public static int countPerGroup = 3;//FOR TEST PURPOSE

        //public static int countPerGroup = 10;

        public static string forgotPasswordUrl = "http://" + GetHost() + "/Account/ForgotPassword";


		public ConstantData()
		{
		}
#if DEBUG_ON_LOCAL
        public const int DefaultStartUnitID = 88;
        public const int DefaultStartSongID = 1;

#else
        public const int DefaultStartSongID = 1;
        public const int DefaultStartUnitID = 4;
        public const int DefaultStartPoetryID = 1747;

#endif
#if TanChang8
        public const string FileServerHost = "m.tanchang8.com";
        private static string debugServer = "m.51math.com";
        private static string dataTestServer = "test.51math.cn";
        //private static string localDebugServer = "local.tanchang8.com";
        private static string localDebugServer = "192.168.1.35";
        //private static string onlineServer = "192.168.1.35";
        public const string localFileServerHost = "192.168.1.35:81";
        private static string onlineServer = "d.tanchang8.com";
#else
        public const string FileServerHost = "m.wellyours.com";
        private static string debugServer = "m.51math.com";
        private static string dataTestServer = "test.51math.cn";
        private static string localDebugServer = "192.168.1.35";
        private static string onlineServer = "wellyours.com";
#endif



        public static string GetHost()
        {
#if DEBUG_ON_LOCAL
            //return onlineServer;
            return localDebugServer;		
#elif INTEGRATION_TEST
            return debugServer;
#elif DATA_TEST
            return dataTestServer;
#else
            return onlineServer;
#endif
        }
        public static string GetFileServerHost()
        {
#if DEBUG_ON_LOCAL
            //return onlineServer;
            return localFileServerHost;
#elif INTEGRATION_TEST
            return debugServer;
#elif DATA_TEST
            return dataTestServer;
#else
            return ConstantData.FileServerHost;
#endif
        }

	}
}
