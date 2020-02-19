/*
 * This code is generated from JAVA.
*/

//using Com.Vjiaoshi;
//using Com.Vjiaoshi.DB;
//using Sharpen;

namespace MVCSite.Common
{
    public enum VisibilityType
    {
        Members = 1,
        Organization,
        Public,
        OnlyMyself
    };
    public enum CommentsRightType
    {
        Disabled = 1,
        Members,
        PublicMembers
    };
    public enum AddMemberRightType
    {
        Admins = 1,
        Members,

    };
    public enum UserRole
    {
        Guider = 0,
        Tourist,
        Agent,
    };
    public enum ConnectPhoneType
    {
        Unknown = 0,
        Mobile = 1,
        Telephone,
    };
    public enum SexType
    {
        Male = 0,
        Female = 1
    };

    public partial class EnumTranslation
    {
        public static string[] VisibilityTypeEn = new string[] 
        { 
            "Members", 
            "Organization", 
            "Public",
            "OnlyMyself",
        };
        public static string[] VisibilityTypeCn = new string[] 
        { 
            "只对队员可见", 
            "团队内部可见", 
            "公开到互联网",
            "只有我能访问"
        };
        public static string[] CommentsRightTypeCn = new string[] 
        { 
            "禁用", 
            "团队成员", 
            "互联网网民",
        };
        public static string[] AddMemberRightTypeCn = new string[] 
        { 
            "管理员", 
            "团队成员", 
        };
        public static string[] ConnectPhoneTypeCn = new string[] 
        { 
            "未知", 
            "手机", 
            "固定电话", 
        };
        public static string[] SexTypeCn = new string[] 
        { 
            "师哥", 
            "学妹", 
        };
        public static string[] SexTypeEn = new string[] 
        { 
            "Male", 
            "Female", 
            "Other", 
        };
    }


    public enum ProgressStatus
    {
        VersionChecked = 1,
        Logoned = 2,
        GotBooks = 3,
        GotBook = 4,
        GotLocalWords = 5,
        GotServerWords = 6,
        GotAudios = 7,
        AudioUnzipped = 8,
        ServerWordsAllDone = 9,
        ReplacedBookDataSubmitted = 10,
        //		ReplacedBookDataCleared(11),	//Merged to BookLocalDataDestroyed
        Registered = 12,
        BookLocalDataDestroyed = 13,
        BookServerDataDestroyed = 14,
        ServerWordsDataDownloaded=15,
    }

    public enum ProgressType
    {
        Idle = 0,
        LogonFromCookie = 1,
        LogonFromUser = 2,//NOT USED Since V2.1.0
        Register = 3,//NOT USED Since V2.1.0
        InitiallySelectBook = 4,
        SwitchBook = 5,
        DestroyBook = 6,
        ReloadHistoryWords = 7,
        SelectWordPitureDictUnit=8,
        LogonFromUserToBrowser=9,
        RegisterToBrowser = 10,
    }
    public enum ClearLocalDataType
    {
        Unknown = 0,
        DestroyBook = 1,
        SwitchToNewBook = 2,
        Logout = 3,
        CheckAndSaveMediaToDBOnStart = 4,
        ReloadHistoryWords = 5,
        SelectWordPitureDictUnit=6,

    }
    public enum OfflineWordsGetType
    {
        Unknown = 0,
        FirstTimeInitiateBook = 1,
        SubmitAndGetNewWords = 2,
        OnlySubmitWhenSwitchBook = 3,
        RefreshSpecificWords = 4,
        LogonInitiateBook = 5,
        SelectWordPitureDictUnit=6,
    }


    public enum MediaSyncType
    {
        Unknown = 0,
        UpdateAccessTimeCreateMediaIfNot = 1,
        ClearMediaAndUpdateFlag = 2,
        UpdateRecitedWords = 3,
        SaveWordMediaToDatabase=4,
        CreateMediaFilesFromDB = 5,
        SaveUnusedUnitMediaFilesToDB = 6,
    }
    public enum ClientLogonMode
    {
        NormalLogon = 0,
        InviteeLogon = 1,
        JoinFavoriteLogon = 2,
        AndroidMobileLogon = 3,
        iPhoneLogon = 4,

        OSXEditor = 100,
        OSXPlayer = 101,
        WindowsPlayer = 102,
        OSXWebPlayer = 103,
        OSXDashboardPlayer = 104,
        WindowsWebPlayer = 105,
        WiiPlayer = 106,
        WindowsEditor = 107,
        //IPhonePlayer = 8,
        PS3 = 109,
        XBOX360 = 110,
        //Android = 11,
        NaCl = 112,
        LinuxPlayer = 113,
        FlashPlayer = 115,
        MetroPlayerX86 = 118,
        MetroPlayerX64 = 119,
        MetroPlayerARM = 120,
        WP8Player = 121,
        BB10Player = 122,


    }
    public enum RequestBooktypeMode
    {
        SwitchToNewBook = 1,
        RegisterSelectBook=2
    }
    public enum ResultType
    {
        UNKNOW = 0,
        OK = 1,
        DONE = 2,
        RedirectToSelectWordChinese = 3,
        RedirectToSpeakSentenceWord = 4,
        RedirectToMySettings = 5,
        RedirectToBooktypeList = 6,
        ChangeAudio = 7,
        Logout = 8,
        Restart = 9,
        RedirectToSpeakImageWord = 10,
        QuitApplication = 11,
        CANCEL = 12,
        TIMEOUT = 13,
        DestroyBook = 14,
        RedirectToAboutUs = 15,
        RedirectToPronouncePractice = 16,
        ShowcaseDone = 17,
        PlayUserlogout = 18,
        RedirectToLogon = 19,
        RedirectToRegister = 20,
        RedirectToListenOutChinese = 21,
        RedirectToSelectImageChinese = 22,
        RedirectToSelectWordEnglish = 23,
        ReloadHistoryWords = 24,
    }

    public enum AudioAccentType
    {
        USAudio = 1,
        UKAudio
    }

    public enum SubmitAnswerIndex
    {
        Initial = 1,
        InMiddleOf = 90,
        FirstHalf = 91,
        SecondHalf = 92
    }

    public enum ReciteType
    {
        WatchMovie = 0,
        EnToCn = 1,
        SpellEnglishWord = 2,
        PronouncePractice = 3,
        SpeakSentenceWord = 4,
        ListenOutChinese = 5,
        SelectImageChinese = 6,
        SpeakImageWord = 7,
        CnToEn = 8
    }

    public enum TermFailCountType
    {
        AlreadyGrasped = 0,
        Wrong1Time = 1,
        Wrong2Times = 2,
        Wrong3AndMoreTimes = 3,
        VIPWord = 99,
        EasyWord = 255
    }

    public enum ReviewType
    {
        NotReview = 0,
        VIPReview = 1,
        Wrong3TimeMore = 2,
        Wrong2Time = 3,
        Wrong1Time = 4,
        Passed = 5,
        EasyWord = 6,
        NormalReview = 7,
        Wrong2TimeAndMore = 8,
        Wrong1TimeAndMore = 9,
        End,
    }

    public enum OfflineWordStatus
    {
        All=-1,
        Initial = 0,
        Modified = 1,
        Submitted = 2
    }

    public enum DownloadDataType
    {
        Initiate = 1,
        OnlyRetrieve = 2,
        Unknown = 3,
        DownloadSongAudio,
        DownloadSongComp,
    }

    public enum GetBookDataType
    {
        InitiateBook = 1,
        GetSummary = 2,
        SwitchBook = 3,
        AutoLogon = 4,
    }

    public enum SubmitAnswerType
    {
        Unknown = 0,
        Section = 1,
        Part = 2,
        SwitchToAnotherReviewType = 3,
        Initiate = 4,
        OfflineWordsInitiate = 5
    }



    public enum OperationStatus
    {
        Idle = 0,
        SelectAnotherBook = 1,
        SendRequestToRefreshBookSummary = 2,
        NotUsed = 3,
        LogonManually = 4
    }



    public enum LogonType
    {
        Unknown = 0,
        CookieLogon = 1,
        ManualLogon = 2,
        TimeoutCookieLogon = 3,
        RegisterLogon = 4
    }



    public enum ActivityEventType
    {
        Unknown = 0,
        Quit = 1,
        SendSelectBookRequest = 2,
        SelectBookResponse = 3,
        SendServerWordsRequest = 4,
        ServerWordsResponse = 5,
        RecognizeRequest = 6,
        RecognizeResponse = 7,
        ScanMediaDataResponse = 8,
        OnActivityResult = 9
    }

    public enum HelpScreenActivity
    {
        Main = 0,
        BookSummary = 1,
        PronouncePractice = 2,
        SelectWordChinese = 3,
        SpeakSentenceWord = 4,
        WordChineseAndUsage = 5,
        MySettings = 6,
        RegisterShowcase = 7,
        BookList = 8,
        SelectWordEnglish = 9,
        ListenOutChinese = 10,
        SelectImageChinese = 11,
        SpeakImageWord = 12
    }
    public enum UserReadTalkStatus
    {
        Read = 0,
        Unread = 1,
    }

}
