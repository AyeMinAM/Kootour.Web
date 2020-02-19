using System;
using System.Collections.Generic;
using System.Text;

namespace MVCSite.Common
{

    public struct FacilitiesStringCons
    {
        public const string Action = "action";
        public const string Actor = "Actor";
        public const string ActorName = "ACTOR_NAME";
        public const string Auditing = "au";
        public const string AutoExit = "autoExit";
        public const string BloggerId = "bloggerId";
        public const string BlogId = "blogId";
        public const string CategoryID = "categoryid";
        public const string CategoryName = "categoryName";
        public const string Certification = "ctft";
        public const string CinemaId = "cinemaId";
        public const string Color = "color";
        public const string CompanyId = "cpy";
        public const string Constellation = "ct";
        public const string Date = "date";
        public const string Day = "day";
        public const string Days = "d";
        public const string Deleted = "del";
        public const string Director = "Director";
        public const string DirectorName = "DIRECTOR_NAME";
        public const string DVDId = "dvdId";
        public const string Email = "email";
        public const string EmailArr = "emails";
        public const string Forget = "forget";
        public const string FristLetter = "fl";
        public const string From = "from";
        public const string Genre = "gn";
        public const string GenreName = "Genre_name";
        public const string GoTo = "GoTo";
        public const string Group_MyGroupListType = "MyGroupListType";
        public const string Guid = "guid";
        public const string Id = "id";
        public const string ID_List = "ID_List";
        public const string ILike = "ilike";
        public const string ImageId = "ImageId";
        public const string ImageType = "imgtp";
        public const string ImageUrl = "imageUrl";
        public const string inviteid = "inviteid";
        public const string Inviter = "inviter";
        public const string KeyWord = "v";
        public const string Language = "lg";
        public const string lastfetch = "lastfetch";
        public const string Location = "lt";
        public const string LocationId = "LocationId";
        public const string LoginEmail = "LoginEmail";
        public const string Month = "month";
        public const string MOVIE_ID = "MOVIE_ID";
        public const string MovieEditPageType = "MovieEditPageType";
        public const string MovieId = "movieid";
        public const string MovieNum = "movienum";
        public const string MyFavoriteTheater = "myFavoriteTheater";
        public const string name = "name";
        public const string NID = "nid";
        public const string ObjectId = "objId";
        public const string ObjectType = "objtypeId";
        public const string OrderBizlogicNo = "OrderBizlogicNo";
        public const string PageID = "pid";
        public const string PageIndex = "start";
        public const string PageName = "pageName";
        public const string PageSubAreaID = "pagesubareaID";
        public const string ParentID = "prid";
        public const string ParentPageIDInTreeLevel = "ParentPageIDInTreeLevel";
        public const string Pass = "password";
        public const string PATH = "PATH";
        public const string PersonId = "personid";
        public const string PhotoID = "photoID";
        public const string Plot = "Plot";
        public const string preview = "preview";
        public const string PropertyTypeID = "ptid";
        public const string RecommendName = "recommendName";
        public const string RedirectUrl = "redirectUrl";
        public const string SearchAll = "all";
        public const string SearchCompany = "company";
        public const string SearchMovie = "movie";
        public const string SearchPerson = "person";
        public const string SearchType = "shtp";
        public const string Sex = "sex";
        public const string ShowtimeId = "showtimeid";
        public const string Sort = "sort";
        public const string Sound = "sound";
        public const string String = "s";
        public const string TagName = "tagname";
        public const string Text = "text";
        public const string Time = "time";
        public const string Timeslice = "timeslice";
        public const string Type = "type";
        public const string Url = "url";
        public const string UserId = "userId";
        public const string UserName = "UserName";
        public const string ValidateCode = "ValidateCode";
        public const string Year = "r";
        public const string YearColumn = "Year";
    }

    public class DateProperty
    {
        //Äê
        private int _Year = 0;
        public int Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        //ÔÂ
        private int _Month = 0;
        public int Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        //ÈÕ
        private int _Day = 0;
        public int Day
        {
            get { return _Day; }
            set { _Day = value; }
        }
    }
    public class NormalHtmlTags
    {
        public const char HTMLTagStartFlag = '<';
        public const char HTMLTagEndFlag = '>';




    }
    #region Math special tag


    public class MathHtmlTags
    {
        public const string SupBeginTag = "<SUP>";
        public const string SupEndTag = "</SUP>";
        public const string SubBeginTag = "<SUB>";
        public const string SubEndTag = "</SUB>";

        public const string LowerSupBeginTag = "<sup>";
        public const string LowerSupEndTag = "</sup>";
        public const string LowerSubBeginTag = "<sub>";
        public const string LowerSubEndTag = "</sub>";

        public const string SupBeginTagReplacement = "__sup_begin__";
        public const string SupEndTagReplacement = "__sup_end__";
        public const string SubBeginTagReplacement = "__sub_begin__";
        public const string SubEndTagReplacement = "__sub_end__";



    }
    #endregion
    public enum FormMethod
    {
        GET,
        POST
    }

    /// <summary>
    /// Í¼Æ¬
    /// </summary>
    public enum PosterImageType
    {
        Origin = 1,
        /// <summary>
        /// ¿íÏÞÖÆ500£¬¸ßÏÞÖÆ500
        /// </summary>
        Clip_500_500 = 31,
        /// <summary>
        /// ±£´æ100 x 140³ß´çÔ¤ÀÀÍ¼ 
        /// </summary>
        Clip_100_140_M3 = 33,
        /// <summary>
        /// Ó°Æ¬¸ÅÀÀÒ³
        /// </summary>
        W75H75 = 4,   // 75 * 75
        Clip_100_140_M2
    }

    public enum WatermarkPositionType
    {
        TopLeft,
        TopCenter,
        TopRight,
        Center,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// Ìí¼ÓË®Ó¡·½Ê½
    /// </summary>
    public enum WatermarkType
    {
        /// <summary>
        /// Ð¡Ë®Ó¡
        /// </summary>
        Small,

        /// <summary>
        /// ½ÏÐ¡Ë®Ó¡
        /// </summary>
        Smaller,

        /// <summary>
        /// ¼«Ð¡Ë®Ó¡
        /// </summary>
        ExtraSmall,

        /// <summary>
        /// ±ê×¼Ë®Ó¡
        /// </summary>
        Standard,
    }

    public enum ImageClipType
    {
        Notset = -1,
        ScaleToFit = 0,
        FixWidthTrimHeight = 1,
        FixWidth = 2,
        FixWidthOrFixHeight = 3,
        FixWidthAndFixHeight = 4,
    }


    public enum Constellation
    {
        Aries = 1,//°×Ñò×ù
        Taurus = 2,//½ðÅ£×ù
        Gemini = 3,//Ë«×Ó×ù
        Cancer = 4,//¾ÞÐ·×ù
        Leo = 5,//Ê¨×Ó×ù
        Virgo = 6,//´¦Å®×ù
        Libra = 7,//Ìì³Ó×ù
        Scorpio = 8,//ÌìÐ«×ù
        Sagittarius = 9,//ÉäÊÖ×ù
        Capricorn = 10,//Ä¦ôÉ×ù
        Aquarius = 11,//Ë®Æ¿×ù
        Pisces = 12,//Ë«Óã×ù
    }

    public struct ConstellationShow
    {
        public const string Aries = "°×Ñò×ù";
        public const string Taurus = "½ðÅ£×ù";
        public const string Gemini = "Ë«×Ó×ù";
        public const string Cancer = "¾ÞÐ·×ù";
        public const string Leo = "Ê¨×Ó×ù";
        public const string Virgo = "´¦Å®×ù";
        public const string Libra = "Ìì³Ó×ù";
        public const string Scorpio = "ÌìÐ«×ù";
        public const string Sagittarius = "ÉäÊÖ×ù";
        public const string Capricorn = "Ä¦ôÉ×ù";
        public const string Aquarius = "Ë®Æ¿×ù";
        public const string Pisces = "Ë«Óã×ù";

    }
    //public enum SiteType
    //{
    //    NotSet = -1,
    //    WWW51Math = 0,

    //}

}
