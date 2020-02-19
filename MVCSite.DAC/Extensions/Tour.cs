using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.IO;
using MVCSite.DAC.Common;
using System.Data;
using MVCSite.Common;
using System.Linq;

namespace MVCSite.DAC.Entities
{
    public enum TourTimeType
    {
        Days = 0,
        Hours,
        Minutes,
       
    };
    public class TimeTypeTranslation
    {
        public int Code;
        public string TimeType;
        public TimeTypeTranslation(TourTimeType code, string lan)
        {
            Code = (int)code;
            TimeType = lan;
        }
        public static List<TimeTypeTranslation> Translations = new List<TimeTypeTranslation>() 
        { 
            new TimeTypeTranslation(TourTimeType.Days, "Days"), 
            new TimeTypeTranslation(TourTimeType.Hours, "Hours"), 
            new TimeTypeTranslation(TourTimeType.Minutes, "Minutes"), 
        };
        public static string GetTranslationOf(TourTimeType code) 
        { 
            TimeTypeTranslation trans=Translations.Where(x=>x.Code == (int)code).SingleOrDefault();
            if(trans == null)
                return string.Empty;
            return trans.TimeType;
        }
    };
    public enum TourStatus
    {
        Incomplete = 0,
        Complete = 1,
        Active = 2,
        Inactive = 3,
        Deleted = 4,
        Published = 5
    };
    public enum TourBookingType
    {
        PerPerson = 0,
        PerGroup = 1,
    };
    public class BookingTypeTranslation
    {
        public int Code;
        public string BookingType;
        public BookingTypeTranslation(TourBookingType code, string lan)
        {
            Code = (int)code;
            BookingType = lan;
        }
        public static List<BookingTypeTranslation> Translations = new List<BookingTypeTranslation>() 
        { 
            new BookingTypeTranslation(TourBookingType.PerPerson, "Per Person"), 
            new BookingTypeTranslation(TourBookingType.PerGroup, "Per Group"), 
        };
        public static string GetTranslationOf(TourBookingType code)
        {
            BookingTypeTranslation trans = Translations.Where(x => x.Code == (int)code).SingleOrDefault();
            if (trans == null)
                return string.Empty;
            return trans.BookingType;
        }
    };
    public enum GenderType
    {
        Male = 0,
        Female = 1,
        Other = 2,
    };
    public enum LanguageCode
    {
        All=0,
        English=1,
        ChineseMandarian=2,
        ChineseCantonese=3,
        French=4,
        Spanish=5,
        German=6,
        Portuguese=7,
        Italian=8,
        Russian=9,
        Korean=10,
        Japanese=11,
        Norwegian=12,
        Swedish=13,
        Danish=14
    }

    public class LanguageTranslation {
        public int Code;
        public string Language;
        public LanguageTranslation(LanguageCode code,string lan)
        {
            Code=(int)code;
            Language=lan;
        }
        public static List<LanguageTranslation> Translations = new List<LanguageTranslation>() 
        { 
            new LanguageTranslation(LanguageCode.All, "All"), 
            new LanguageTranslation(LanguageCode.English, "English"),              
            new LanguageTranslation(LanguageCode.ChineseMandarian, "Mandarin"),
            new LanguageTranslation(LanguageCode.ChineseCantonese, "Cantonese"), 
            new LanguageTranslation(LanguageCode.French, "French"), 
            new LanguageTranslation(LanguageCode.Spanish, "Spanish"), 
            new LanguageTranslation(LanguageCode.German, "German"), 
            new LanguageTranslation(LanguageCode.Portuguese, "Portuguese"), 
            new LanguageTranslation(LanguageCode.Italian, "Italian"), 
            new LanguageTranslation(LanguageCode.Russian, "Russian"), 
            new LanguageTranslation(LanguageCode.Korean, "Korean"), 
            new LanguageTranslation(LanguageCode.Japanese, "Japanese"),
            new LanguageTranslation(LanguageCode.Norwegian, "Norwegian"),
            new LanguageTranslation(LanguageCode.Swedish, "Swedish"),
            new LanguageTranslation(LanguageCode.Danish, "Danish")
        };
    };
    public enum TourCategory
    {
        All = 1,
        Adventure = 2,
        CultureArts = 3,
        FestivalEvents = 4,
        FoodDrink = 5,
        Historical = 6,
        LeisureSports = 7,
        NatureRural = 8,
        NightlifeParty = 9,
        ShoppingMarket = 10,
        Transportation = 11,
        BusinessInterpretation = 12,
        Photography = 13
    }
    public class TourCategoryTranslation
    {
        public int Code;
        public string Category;
        public TourCategoryTranslation(TourCategory code, string cat)
        {
            Code = (int)code;
            Category = cat;
        }
        public static List<TourCategoryTranslation> Translations = new List<TourCategoryTranslation>() 
        { 
            new TourCategoryTranslation(TourCategory.All, "Select Category"), 
            new TourCategoryTranslation(TourCategory.Adventure, "Adventure"), 
            new TourCategoryTranslation(TourCategory.CultureArts, "Culture & Arts"),
            new TourCategoryTranslation(TourCategory.FestivalEvents, "Festival & Events"), 
            new TourCategoryTranslation(TourCategory.FoodDrink, "Food & Drink"), 
            new TourCategoryTranslation(TourCategory.Historical, "Historical"), 
            new TourCategoryTranslation(TourCategory.LeisureSports, "Leisure & Sports"), 
            new TourCategoryTranslation(TourCategory.NatureRural, "Nature & Rural"), 
            new TourCategoryTranslation(TourCategory.NightlifeParty, "Nightlife & Party"), 
            new TourCategoryTranslation(TourCategory.ShoppingMarket, "Shopping & Market"),
            new TourCategoryTranslation(TourCategory.Transportation, "Transportation"),
            new TourCategoryTranslation(TourCategory.BusinessInterpretation, "Business & Interpretation"),
            new TourCategoryTranslation(TourCategory.Photography, "Photography")
        };
    };


   

public class SearchCriteria
    {
        public int Cityid;
        public DateTime OnDate;
        public bool IsFeaturedTour;
        public int Duration;

        public bool IsAllLanguage;
        public bool IsEnglish;
        public bool IsChineseMandarian;
        public bool IsChineseCantonese;
        public bool IsFrench;
        public bool IsSpanish;
        public bool IsGerman;
        public bool IsPortuguese;
        public bool IsItalian;
        public bool IsRussian;
        public bool IsKorean;
        public bool IsJapanese;
        public bool IsNorwegian;
        public bool IsSwedish;
        public bool IsDanish;

        public bool IsAllCategory;
        public bool IsHistorical;
        public bool IsAdventure;
        public bool IsLeisureSports;
        public bool IsCultureArts;
        public bool IsNatureRural;
        public bool IsFestivalEvents;
        public bool IsNightlifeParty;
        public bool IsFoodDrink;
        public bool IsShoppingMarket;
        public bool IsTransportation;
        public bool IsBusinessInterpretation;
        public bool IsPhotography;

        public SearchCriteria() { 
            Cityid =0;
            OnDate = DateTime.UtcNow;
            IsFeaturedTour = false;
            Duration = 1;

            IsAllLanguage = false;
            IsEnglish = false;
            IsChineseMandarian = false;
            IsChineseCantonese = false;
            IsFrench = false;
            IsSpanish = false;
            IsGerman = false;
            IsPortuguese = false;
            IsItalian = false;
            IsRussian = false;
            IsKorean = false;
            IsJapanese = false;
            IsNorwegian = false;
            IsSwedish = false;
            IsDanish = false;

            IsAllCategory = false;
            IsHistorical = false;
            IsAdventure = false;
            IsLeisureSports = false;
            IsCultureArts = false;
            IsNatureRural = false;
            IsFestivalEvents = false;
            IsNightlifeParty = false;
            IsFoodDrink = false;
            IsShoppingMarket = false;
            IsTransportation = false;
            IsBusinessInterpretation = false;
            IsPhotography = false;
        }
    };
    public partial class Tour
    {
        public string City { get; set; }
        #region Constructors
        public Tour()
        {
            this.ID = 0;
            this.Name = string.Empty;
            this.IsHistorical = false;
            this.IsAdventure = false;
            this.IsLeisureSports = false;
            this.IsCultureArts = false;
            this.IsNatureRural = false;
            this.IsFestivalEvents = false;
            this.IsNightlifeParty = false;
            this.IsFoodDrink = false;
            this.IsShoppingMarket = false;
            this.IsTransportation = false;
            this.IsBusinessInterpretation = false;
            this.IsPhotography = false;

            this.IsEnglish = false;
            this.IsChineseMandarian = false;
            this.IsChineseCantonese = false;
            this.IsFrench = false;
            this.IsSpanish = false;
            this.IsGerman = false;
            this.IsPortuguese = false;
            this.IsItalian = false;
            this.IsRussian = false;
            this.IsKorean = false;
            this.IsJapanese = false;
            this.IsNorwegian = false;
            this.IsSwedish = false;
            this.IsDanish = false;

            this.Overview = string.Empty;
            this.Itinerary = string.Empty;
            this.Duration = 0;
            this.TourCityID = 0;
            this.MeetupLocation = string.Empty;
            this.MinTouristNum = 0;
            this.MaxTouristNum = 0;
            this.BookingType = 0;
            this.MinHourAdvance = 0;

            this.Status = (byte)TourStatus.Incomplete;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
            this.UserID = 0;
            this.DurationTimeType = 0;
        }
        public Tour(Tour src)
        {
            this.ID = src.ID;
            this.Name = src.Name;
            this.IsHistorical = src.IsHistorical;
            this.IsAdventure = src.IsAdventure;
            this.IsLeisureSports = src.IsLeisureSports;
            this.IsCultureArts = src.IsCultureArts;
            this.IsNatureRural = src.IsNatureRural;
            this.IsFestivalEvents = src.IsFestivalEvents;
            this.IsNightlifeParty = src.IsNightlifeParty;
            this.IsFoodDrink = src.IsFoodDrink;
            this.IsShoppingMarket = src.IsShoppingMarket;
            this.IsTransportation = src.IsTransportation;
            this.IsBusinessInterpretation = src.IsBusinessInterpretation;
            this.IsPhotography = src.IsPhotography;

            this.IsEnglish = src.IsEnglish;
            this.IsChineseMandarian = src.IsChineseMandarian;
            this.IsChineseCantonese = src.IsChineseCantonese;
            this.IsFrench = src.IsFrench;
            this.IsSpanish = src.IsSpanish;
            this.IsGerman = src.IsGerman;
            this.IsPortuguese=src.IsPortuguese;
            this.IsItalian = src.IsItalian;
            this.IsRussian = src.IsRussian;
            this.IsKorean = src.IsKorean;
            this.IsJapanese = src.IsJapanese;
            this.IsNorwegian = src.IsNorwegian;
            this.IsSwedish = src.IsSwedish;
            this.IsDanish = src.IsDanish;


            this.Overview = src.Overview;
            this.Itinerary = src.Itinerary;
            this.Duration = src.Duration;
            this.TourCityID = src.TourCityID;
            this.MeetupLocation =src.MeetupLocation;
            this.MinTouristNum = src.MinTouristNum;
            this.MaxTouristNum = src.MaxTouristNum;
            this.BookingType =src.BookingType;
            this.MinHourAdvance = src.MinHourAdvance;

            this.Status = src.Status;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
            this.UserID = src.UserID;
            this.DurationTimeType = src.DurationTimeType;

            this.ReviewCount = src.ReviewCount;
            this.ReviewAverageScore = src.ReviewAverageScore;
            
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public Tour(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.Name = loader.LoadString("Name");
            this.IsHistorical = loader.LoadBoolean("IsHistorical");
            this.IsAdventure = loader.LoadBoolean("IsAdventure");
            this.IsLeisureSports = loader.LoadBoolean("IsHistorical");
            this.IsCultureArts = loader.LoadBoolean("IsCultureArts");
            this.IsNatureRural = loader.LoadBoolean("IsNatureRural");
            this.IsFestivalEvents = loader.LoadBoolean("IsFestivalEvents");
            this.IsNightlifeParty = loader.LoadBoolean("IsNightlifeParty");
            this.IsFoodDrink = loader.LoadBoolean("IsFoodDrink");
            this.IsShoppingMarket = loader.LoadBoolean("IsShoppingMarket");
            this.IsTransportation = loader.LoadBoolean("IsTransportation");
            this.IsBusinessInterpretation = loader.LoadBoolean("IsBusinessInterpretation");
            this.IsPhotography = loader.LoadBoolean("IsPhotography");

            this.IsEnglish = loader.LoadBoolean("IsEnglish");
            this.IsChineseMandarian = loader.LoadBoolean("IsChineseMandarian");
            this.IsChineseCantonese = loader.LoadBoolean("IsChineseCantonese");
            this.IsFrench = loader.LoadBoolean("IsFrench"); 
            this.IsSpanish = loader.LoadBoolean("IsSpanish");
            this.IsGerman = loader.LoadBoolean("IsGerman");
            this.IsPortuguese = loader.LoadBoolean("IsPortuguese");
            this.IsItalian = loader.LoadBoolean("IsItalian");
            this.IsRussian = loader.LoadBoolean("IsRussian");
            this.IsKorean = loader.LoadBoolean("IsKorean");
            this.IsJapanese = loader.LoadBoolean("IsJapanese");
            this.IsNorwegian = loader.LoadBoolean("IsNorwegian");
            this.IsSwedish = loader.LoadBoolean("IsSwedish");
            this.IsDanish = loader.LoadBoolean("IsDanish");


            this.Overview = loader.LoadString("Overview");
            this.Itinerary = loader.LoadString("Itinerary");
            this.Duration = loader.LoadInt32("Duration");
            this.TourCityID = loader.LoadInt32("TourCityID");
            this.MeetupLocation = loader.LoadString("MeetupLocation");
            this.MinTouristNum = loader.LoadInt32("MinTouristNum");
            this.MaxTouristNum = loader.LoadInt32("MaxTouristNum");
            this.BookingType = loader.LoadByte("BookingType");
            this.MinHourAdvance = loader.LoadInt32("MinHourAdvance");

            this.Status = loader.LoadByte("Status");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
            this.UserID = loader.LoadInt32("UserID");
            this.DurationTimeType = loader.LoadByte("DurationTimeType");

            this.ReviewCount = loader.LoadInt32("ReviewCount");
            this.ReviewAverageScore = loader.LoadDouble("ReviewAverageScore");
                   
        }

        #endregion
        public TourStatus RealStatus
        {
            get { return (TourStatus)Status; }
            set { Status = (byte)value; }
        }
        public TourBookingType RealBookingType
        {
            get { return (TourBookingType)BookingType; }
            set { BookingType = (byte)value; }
        }
        public string CoverImageUrl
        {
            get {
                if (string.IsNullOrEmpty(ImagePath))
                    return string.Empty;
                return Path.Combine(StaticSiteConfiguration.ImageServerUrl, ImagePath);
            }
        }
        public string ShowPrice
        {
            get;
            set;
        }
        public string ShowDiscount
        {
            get;
            set;
        }
        public string ShowPerType
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public double SugRetailPrice
        {
            get;
            set;
        }
        public double NowPrice
        {
            get;
            set;
        }
        public string ImagePath
        {
            get;
            set;
        }
        public string PerPersonOrGroup
        {
            get
            {
                return BookingTypeTranslation.GetTranslationOf((TourBookingType)BookingType); ;
            }
        }
        public bool IsDataSaved
        {
            get;
            set;
        }
    }
}
