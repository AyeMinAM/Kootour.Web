using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Services;
using MVCSite.Web.ViewModels;
using MVCSite.Biz;
using MVCSite.DAC.Common;
using MVCSite.Biz.Interfaces;
using Microsoft.Practices.Unity;
using DevTrends.MvcDonutCaching;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
using System.Web.Script.Serialization;
using MVCSite.Web.Extensions;
using MVCSite.ViewResource;
using System.Collections;
using MVCSite.DAC.Extensions;
using System.Globalization;
using MVCSite.Web.Services;

namespace MVCSite.Web.Controllers
{
    public class GuideController : LayoutBase
    {
        private readonly IPublicCommands _commands;
        protected readonly IRepositoryGuides _repositoryGuides;
        protected readonly IRepositoryCities _repositoryCities;
        private readonly GuideService _guideService;
        public GuideController(
            ISecurity security, 
            IWebApplicationContext webContext,
            IRepositoryUsers repositoryUsers,
            IRepositoryGuides repositoryGuides,
            IRepositoryCities repositoryCities,
            IPublicCommands commands,
            ISiteConfiguration configuration, 
            ILogger logger)
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
            _commands = commands;
            _repositoryGuides = repositoryGuides;
            _repositoryCities = repositoryCities;

            _guideService = new GuideService(
                security,
                webContext,
                repositoryUsers,
                repositoryGuides,
                repositoryCities,
                commands,
                configuration,
                logger);
        }



        //[DonutOutputCache(CacheProfile = "OneDay")]
        [Authorize]
        public ActionResult TourType(Nullable<int> id)
        {
            return _guideService.ShowTourType(id);
            //Tour tour = null;
            //if (id != null)
            //{
            //    tour = _repositoryGuides.TourGetByID(id.Value);
            //}
            //else
            //{
            //    tour = _repositoryGuides.TourGetByUserIDStatus(_security.GetCurrentUserId(), TourStatus.Incomplete);
            //}
            //int[] lanIds = new int[] { (int)LanguageCode.English };
            //if(tour !=null)
            //{
            //    var tourLans=new List<int>();
            //    bool[] lanFlags = new bool[] {
            //        tour.IsEnglish,
            //        tour.IsChineseMandarian,
            //        tour.IsChineseCantonese,
            //        tour.IsFrench,
            //        tour.IsSpanish,
            //        tour.IsGerman,
            //        tour.IsPortuguese,
            //        tour.IsItalian,
            //        tour.IsRussian,
            //        tour.IsKorean,
            //        tour.IsJapanese,                
            //    };
            //    for (int i = 0; i < lanFlags.Length; i++)
            //    {
            //        if (lanFlags[i])
            //            tourLans.Add(i+1);
            //    }
            //    lanIds = tourLans.ToArray();
            //}
            //var model = new TourTypeModel
            //{
            //    IsMember = false,
            //    IsSignedIn = false,
            //    SelectedPage = LayoutSelectedPage.Account,
            //    Name = tour == null?string.Empty:tour.Name,
            //    IsHistorical=tour==null?false:tour.IsHistorical,
            //    IsAdventure = tour == null ? false : tour.IsAdventure,
            //    IsLeisureSports = tour == null ? false : tour.IsLeisureSports,
            //    IsCultureArts = tour == null ? false : tour.IsCultureArts,
            //    IsNatureRural = tour == null ? false : tour.IsNatureRural,
            //    IsFestivalEvents = tour == null ? false : tour.IsFestivalEvents,
            //    IsNightlifeParty = tour == null ? false : tour.IsNightlifeParty,
            //    IsFoodDrink = tour == null ? false : tour.IsFoodDrink,
            //    IsShoppingMarket = tour == null ? false : tour.IsShoppingMarket,
            //    IsTransportation = tour == null ? false : tour.IsTransportation,
            //    IsBusinessInterpretion = tour == null ? false : tour.IsBusinessInterpretion,
            //    LanguageIDs = lanIds,
            //    LanguageOptions = LanguageTranslation.Translations.Skip(1).ToSelectList(x => x.Language, x => x.Code.ToString()),
            //    ID = tour == null ? 0 :tour.ID,
            //};
            //return View("TourType", InitLayout(model));

        }

        [HttpPost]
        public ActionResult TourType(TourTypeModel model)
        {
            try
            {
                
                ValidTourType(model);
                if (ModelState.IsValid)
                {
                    return _guideService.SaveTourType(model);

                    //    var tour = _repositoryGuides.TourGetByID(model.ID);
                    //    if (tour == null)
                    //    {
                    //        tour = new Tour();
                    //    }
                    //    tour.UserID = _security.GetCurrentUserId();
                    //    tour.Name = model.Name;
                    //    tour.IsHistorical = model.IsHistorical;
                    //    tour.IsAdventure = model.IsAdventure;
                    //    tour.IsLeisureSports = model.IsLeisureSports;
                    //    tour.IsCultureArts = model.IsCultureArts;
                    //    tour.IsNatureRural = model.IsNatureRural;
                    //    tour.IsFestivalEvents = model.IsFestivalEvents;
                    //    tour.IsNightlifeParty = model.IsNightlifeParty;
                    //    tour.IsFoodDrink = model.IsFoodDrink;
                    //    tour.IsShoppingMarket = model.IsShoppingMarket;
                    //    tour.IsTransportation = model.IsTransportation;
                    //    tour.IsBusinessInterpretion = model.IsBusinessInterpretion;

                    //    tour.IsEnglish = model.LanguageIDs.Contains((int)LanguageCode.English);
                    //    tour.IsChineseMandarian = model.LanguageIDs.Contains((int)LanguageCode.ChineseMandarian);
                    //    tour.IsChineseCantonese = model.LanguageIDs.Contains((int)LanguageCode.ChineseCantonese);
                    //    tour.IsFrench = model.LanguageIDs.Contains((int)LanguageCode.French);
                    //    tour.IsSpanish = model.LanguageIDs.Contains((int)LanguageCode.Spanish);
                    //    tour.IsGerman = model.LanguageIDs.Contains((int)LanguageCode.German);
                    //    tour.IsPortuguese = model.LanguageIDs.Contains((int)LanguageCode.Portuguese);
                    //    tour.IsItalian = model.LanguageIDs.Contains((int)LanguageCode.Italian);
                    //    tour.IsRussian = model.LanguageIDs.Contains((int)LanguageCode.Russian);
                    //    tour.IsKorean = model.LanguageIDs.Contains((int)LanguageCode.Korean);
                    //    tour.IsJapanese = model.LanguageIDs.Contains((int)LanguageCode.Japanese);   

                    //    tour.ModifyTime = DateTime.UtcNow;
                    //    _repositoryGuides.TourCreateOrUpdate(tour);
                    //    return RedirectToAction("Overview", new { id = tour.ID});
                }
                model.LanguageOptions = LanguageTranslation.Translations.Skip(1).ToSelectList(x => x.Language, x => x.Code.ToString());
                return View("TourType", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError","Static",null);
            }
        }
        private void ValidTourType(TourTypeModel model)
        {
            int typeCount = 0;
            bool[] types = new bool[] {
                model.IsHistorical,model.IsAdventure,model.IsLeisureSports,
                model.IsCultureArts,model.IsNatureRural,model.IsFestivalEvents,
                model.IsNightlifeParty,model.IsFoodDrink,model.IsShoppingMarket,
                model.IsTransportation,model.IsBusinessInterpretation,model.IsPhotography
            };
            foreach(var type in types)
            {
                if (type)
                {
                    typeCount++;
                }
            }
            if (model.LanguageIDs == null ||model.LanguageIDs.Length<=0)
            {
                ModelState.AddModelError("LanguageValidMsg", ValidationStrings.TourLanguageSelection);
            }
            if (typeCount <= 0 || typeCount >= 6)
            {
                ModelState.AddModelError("TypeValidMsg", ValidationStrings.TourTypeSelection);
            }
            return;
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult Overview(Nullable<int> id)
        {
            return _guideService.ShowOverview(id);
            //Tour tour = null;
            //List<TourExclusion> tourExclusions = null;
            //List<TourInclusion> tourInclusions = null;
            //if (id != null)
            //{
            //    tour = _repositoryGuides.TourGetByID(id.Value);
            //    tourExclusions = _repositoryGuides.TourExclusionGetAllByTourID(id.Value).OrderBy(x=>x.SortNo).ToList();
            //    tourInclusions = _repositoryGuides.TourInclusionGetAllByTourID(id.Value).OrderBy(x => x.SortNo).ToList();
            //}
            //int inclusionCount=tourInclusions != null ?tourInclusions.Count :0;
            //int exclusionCount = tourExclusions != null ? tourExclusions.Count : 0;
            //string cityString = string.Empty;
            //if (tour != null)
            //{
            //    var city = _repositoryCities.GetCityByIdInDB(tour.TourCityID);
            //    if(city !=null)
            //    {
            //        cityString = city.UniqueCityName;
            //    }
            //}
            //var model = new TourOverviewModel
            //{
            //    IsMember = false,
            //    IsSignedIn = false,
            //    SelectedPage = LayoutSelectedPage.Account,
            //    Overview = tour == null ? string.Empty : tour.Overview,
            //    Itinerary = tour == null ? string.Empty : tour.Itinerary,
            //    Duration = tour == null ? 0 :tour.Duration,
            //    DurationType = tour == null ? (int)TourTimeType.Days : tour.DurationTimeType,
            //    DurationTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString()),
            //    //TourCityID = tour == null ? 0 : tour.TourCityID,
            //    //CityOptions=_repositoryCities.GetCitiesSelectListItemCached(),
            //    TourCity=cityString,
            //    TourCityHidden=cityString,
            //    MeetupLocation = tour == null ? string.Empty : tour.MeetupLocation,
                
            //    Inclusion1 = inclusionCount > 0?tourInclusions[0].Name:string.Empty,
            //    Inclusion2 = inclusionCount > 1 ? tourInclusions[1].Name : string.Empty,
            //    Inclusion3 = inclusionCount > 2 ? tourInclusions[2].Name : string.Empty,
            //    Inclusion4 = inclusionCount > 3 ? tourInclusions[3].Name : string.Empty,
            //    Inclusion5 = inclusionCount > 4 ? tourInclusions[4].Name : string.Empty,

            //    Exclusion1 = exclusionCount > 0 ? tourExclusions[0].Name : string.Empty,
            //    Exclusion2 = exclusionCount > 1 ? tourExclusions[1].Name : string.Empty,
            //    Exclusion3 = exclusionCount > 2 ? tourExclusions[2].Name : string.Empty,
            //    Exclusion4 = exclusionCount > 3 ? tourExclusions[3].Name : string.Empty,
            //    Exclusion5 = exclusionCount > 4 ? tourExclusions[4].Name : string.Empty,
            //    ID = tour == null ? 0 : tour.ID,

            //};
            //return View("Overview", InitLayout(model));

        }
        [HttpPost]
        public ActionResult Overview(TourOverviewModel model)
        {
            try
            {
                ValidTourOverview(model);
                if (ModelState.IsValid)
                {
                    return _guideService.SaveOverview(model);
                    //var cityInUrl = string.Empty;
                    //City city = null;
                    //int cityId = 0;
                    //if (!string.IsNullOrEmpty(model.TourCityHidden))
                    //{
                    //    cityInUrl = model.TourCityHidden.Replace(",", "-").ToLower();
                    //    city = _repositoryCities.GetCityByNameInUrlOrNullInDB(cityInUrl);
                    //    if (city == null)
                    //    {
                    //        var parts = model.TourCityHidden.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //        if (parts == null)
                    //        {
                    //            ModelState.AddModelError("TourCity", "Please input a valid city");
                    //        }
                    //        else
                    //        {
                    //            int partLen = parts.Length;
                    //            if (partLen != 3)
                    //            {
                    //                ModelState.AddModelError("TourCity", "Please input a valid city");
                    //            }
                    //            else
                    //            {
                    //                var country = _repositoryCities.CountryCreateOrGet(parts[2]);
                    //                var region = _repositoryCities.RegionCreateOrGet(parts[1], country.country_id);
                    //                city = _repositoryCities.CityCreateOrGet(parts[0], model.TourCityHidden,
                    //                    cityInUrl, country.country_id, region.Id);
                    //                cityId = city.CityId;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        cityId = city.CityId;
                    //    }
                    //}
                    //var tour = _repositoryGuides.TourGetByID(model.ID);
                    //if (tour == null)
                    //{
                    //    tour = new Tour();
                    //    tour.UserID = _security.GetCurrentUserId();
                    //}
                    //tour.Overview = model.Overview;
                    //tour.Itinerary = model.Itinerary;
                    //tour.Duration = model.Duration;
                    //tour.DurationTimeType =(byte)model.DurationType;
                    //tour.TourCityID = cityId;
                    //tour.MeetupLocation = model.MeetupLocation;
                    //tour.ModifyTime = DateTime.UtcNow;
                    //tour = _repositoryGuides.TourCreateOrUpdate(tour);
                    //var inclusions = new List<string>() { model.Inclusion1, model.Inclusion2, 
                    //    model.Inclusion3, model.Inclusion4, model.Inclusion5 };
                    //var exclusions = new List<string>() { model.Exclusion1, model.Exclusion2, 
                    //    model.Exclusion3, model.Exclusion4, model.Exclusion5 };
                    //for (byte i = 0; i < inclusions.Count; i++)
                    //{
                    //    CheckAndSaveTourInclusion(tour.ID, (byte)(i+1),inclusions[i]);
                    //}
                    //for (int i = 0; i < exclusions.Count; i++)
                    //{
                    //    CheckAndSaveTourExclusion(tour.ID, (byte)(i + 1), exclusions[i]);
                    //}
                    //return RedirectToAction("BookingDetails", new { id = tour.ID }); 

                }
                //model.CityOptions=_repositoryCities.GetCitiesSelectListItemCached();
                model.DurationTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString());
                return View("Overview", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void CheckAndSaveTourInclusion(int tourId,byte sortNo, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var tourInclusion = _repositoryGuides.TourInclusionGetByTourIDSortNo(tourId, sortNo);
                if (tourInclusion == null)
                {
                    tourInclusion = new TourInclusion()
                    {
                        Name = name,
                        SortNo = sortNo,
                        TourID = tourId,
                        EnterTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow,
                    };
                }
                else
                {
                    tourInclusion.Name = name;
                    tourInclusion.ModifyTime = DateTime.UtcNow;
                }
                _repositoryGuides.TourInclusionCreateOrUpdate(tourInclusion);
            }        
        }
        private void CheckAndSaveTourExclusion(int tourId, byte sortNo, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var tourExclusion = _repositoryGuides.TourExclusionGetByTourIDSortNo(tourId, sortNo);
                if (tourExclusion == null)
                {
                    tourExclusion = new TourExclusion()
                    {
                        Name = name,
                        SortNo = sortNo,
                        TourID = tourId,
                        EnterTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow,
                    };
                }
                else
                {
                    tourExclusion.Name = name;
                    tourExclusion.ModifyTime = DateTime.UtcNow;
                }
                _repositoryGuides.TourExclusionCreateOrUpdate(tourExclusion);
            }
        }
        private void ValidTourOverview(TourOverviewModel model)
        {
            if (string.IsNullOrEmpty(model.TourCityHidden) && string.IsNullOrEmpty(model.TourCity))
            {
                ModelState.AddModelError("TourCity", string.Format(ValidationStrings.Required, "TourCity"));
            }

            int i = 0;
            foreach (var v in model.TourInclusionsExtra)
            {
                if (v?.Name?.Length < 1 || v?.Name?.Length > 300)
                {
                    ModelState.AddModelError("TourInclusionExtraValidationMessage" + i, GuideStrings.TourInclusionExtraValidationMessage);
                }
                i++;
            }

            i = 0;
            foreach (var v in model.TourExclusionsExtra)
            {
                if (v?.Name?.Length < 1 || v?.Name?.Length > 300)
                {
                    ModelState.AddModelError("TourExclusionExtraValidationMessage" + i, GuideStrings.TourExclusionExtraValidationMessage);
                }
                i++;
            }
            return;
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult OverviewActive(Nullable<int> id)
        {
            try
            {
                var tour = _repositoryGuides.TourGetByID(id.Value);
                var tourSchedule=_repositoryGuides.TourScheduleGetByTourID(id.Value);
                var model = new TourOverviewActiveModel()
                {
                    Overview=tour.Overview,
                    Itinerary=tour.Itinerary,
                    EndDate = tourSchedule.EndDate.ToString(TimeHelper.DefaultDateFormat),
                    TourID=id.Value
                };
                return View("OverviewActive", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        [HttpPost]
        public ActionResult OverviewActive(TourOverviewActiveModel model)
        {
            try
            {
                var tour = _repositoryGuides.TourGetByID(model.TourID);
                var tourSchedule = _repositoryGuides.TourScheduleGetByTourID(model.TourID);
                tour.Overview = model.Overview;
                tour.Itinerary = model.Itinerary;
                tour.ModifyTime = DateTime.UtcNow;
                _repositoryGuides.TourCreateOrUpdate(tour);
                tourSchedule.EndDate = DateTime.ParseExact(model.EndDate, TimeHelper.DefaultDateFormat, CultureInfo.InvariantCulture);
                tourSchedule.DateRange = string.Format("{0} - {1}",
                    tourSchedule.BeginDate.ToString( TimeHelper.DefaultDateFormat),
                    tourSchedule.EndDate.ToString( TimeHelper.DefaultDateFormat)
                );
                tourSchedule.ModifyTime = DateTime.UtcNow;
                _repositoryGuides.TourScheduleCreateOrUpdate(tourSchedule);
                return RedirectToAction("TourProducts");
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }


        public ActionResult BookingDetails(Nullable<int> id)
        {
            return _guideService.ShowBookingDetails(id);
            //Tour tour = null;
            //if (id != null)
            //{
            //    tour = _repositoryGuides.TourGetByID(id.Value);
            //}
            //var model = new BookingDetailsModel
            //{
            //    IsMember = false,
            //    IsSignedIn = false,
            //    SelectedPage = LayoutSelectedPage.Account,
            //    MinTouristNum = tour == null ? 0 : tour.MinTouristNum,
            //    MaxTouristNum = tour == null ? 0 : tour.MaxTouristNum,

            //    BookingType = tour == null ? (byte)0: tour.BookingType,
            //    MinHourAdvance = tour == null ? 0 : tour.MinHourAdvance,
            //    ID = tour == null ? 0 : tour.ID,

            //};
            //return View("BookingDetails", InitLayout(model));

        }
        [HttpPost]
        public ActionResult BookingDetails(BookingDetailsModel model)
        {
            try
            {
                ValidBookingDetails(model);
                if (ModelState.IsValid)
                {
                    return _guideService.SaveBookingDetails(model);
                    //var tour = _repositoryGuides.TourGetByID(model.ID);
                    //if (tour == null)
                    //    tour = new Tour();
                    //tour.MinTouristNum = model.MinTouristNum;
                    //tour.MaxTouristNum = model.MaxTouristNum;
                    //tour.BookingType = model.BookingType;
                    //tour.MinHourAdvance = model.MinHourAdvance;
                    //tour.ModifyTime = DateTime.UtcNow;
                    //_repositoryGuides.TourCreateOrUpdate(tour);
                    //return RedirectToAction("SchedulerPrice", new { id = tour.ID });
                }
                return View("BookingDetails", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidBookingDetails(BookingDetailsModel model)
        {
            if (model.MinTouristNum <= 0 || model.MinTouristNum > model.MaxTouristNum)
            {
                ModelState.AddModelError("MinTouristNum", ValidationStrings.MinimumTouristNo);
            }
            if (model.MaxTouristNum <= 0 || model.MinTouristNum > model.MaxTouristNum)
            {
                ModelState.AddModelError("MaxTouristNum", ValidationStrings.MaximumTouristNo);
            }
            if (model.MinHourAdvance <= 0 )
            {
                ModelState.AddModelError("MinHourAdvance", ValidationStrings.MinHourAdvance);
            }
            return;
        }
       
        
        public ActionResult ReceivedBookingDetails(int id,int tourId,int touristId)
        {
            try
            {
                Tour tourObject = null;
                List<TourExclusion> tourExclusionList = null;
                List<TourInclusion> tourInclusionList = null;
                List<TourPicture> tourPictureList = null;
                List<TourExtra> tourExtraList = null;
                List<TourPriceBreakdown> tourPriceBreakdownList = null;
                TourSchedule tourScheduleObject = null;
                List<TourVendorPromo> tourVendorPromoList = null;
                List<UserTourBooking> userTourBooking = null;
                List<User> userList = null;
                RepositoryBigQueries.TourGetAllInfoByID(tourId, DateTime.UtcNow.ToString(TimeHelper.DefaultDateFormat), out tourObject, out tourExclusionList, out tourInclusionList, out tourPictureList,
                    out tourExtraList, out tourPriceBreakdownList, out tourScheduleObject, out tourVendorPromoList, out userTourBooking, out userList);

                UserTourBooking currBooking=_repositoryGuides.UserTourBookingGetByID(id);

                var extraIds = currBooking.ExtraIds.SplitToIntArray(",");
                List<TourExtra> bookingExtraList = null;
                if (extraIds != null && extraIds.Count() > 0)
                {
                    bookingExtraList = _repositoryGuides.TourExtraGetAllByIds(extraIds).ToList();
                }
                else
                {
                    bookingExtraList = new List<TourExtra>();
                }

                var tourCity = _repositoryCities.GetCityByIdInDB(tourObject.TourCityID);
                var totalTravelers = currBooking.Travellers;
                var traveler = _repositoryUsers.GetByIdOrNull(currBooking.UserID);
                var guider = _repositoryUsers.GetByIdOrNull(currBooking.TourUserID);
                var tour = _repositoryGuides.TourGetByID(tourId);
                var model = new BookingModel
                {
                    TourName = tourObject.Name,
                    Duration = TourHelper.GetDurationTimeString(tourObject.Duration, tourObject.DurationTimeType),
                    Location = _repositoryCities.GetCityByIdInDB(tourObject.TourCityID).Name,
                    MeetupLocation = tourObject.MeetupLocation,

                    TravelerName = traveler.ShowName,
                    TravelerAddress = traveler.Address,
                    TravelerGender = traveler.GetSexString(GetCurrentLanguage()),
                    TravelerEmail = traveler.Email,
                    //Phone = tourist.PhoneLocalCode,
                    TravelerPhoneAreaCode = traveler.PhoneAreaCode,
                    TravelerMobile = traveler.Mobile,

                    Date = currBooking.Calendar,
                    Time = currBooking.Time,
                    //PackagePriceperPerson = (int)(tourScheduleObject.SugRetailPrice??0),
                    //PackagePriceperGroup = (int)(tourScheduleObject.SugRetailPrice??0),

                    BookingType = tourObject.BookingType,
                    TotalTravelers = totalTravelers,
                    TotalCost = (float)tourScheduleObject?.SugRetailPrice,
                    TourCost = (float)tourScheduleObject.SugRetailPrice,
                    VendorPromoTourCost = (float)PriceService.CalculateVendorPromotedPrice((float)tourScheduleObject.SugRetailPrice, tourVendorPromoList),
                    Extras = bookingExtraList?.Select(x => new BookingConfirmationTourExtraInfo()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Price = x.Price * totalTravelers,
                        Times = x.Time,
                        TimeType = (TourTimeType)x.TimeType
                    }).ToList(),
                    IfShowBookingExtras = bookingExtraList?.Where(x => x.Name.Trim() != "").Count<TourExtra>() > 0,
                    TourPriceBreakdown = tourPriceBreakdownList?.Select(x => new TourPriceBreakdown()
                    {
                        ID = x.ID,
                        TourID = x.TourID,
                        EndPoint1 = x.EndPoint1,
                        EndPoint2 = x.EndPoint2,
                        DiscountValue = x.DiscountValue,
                        DiscountPercent = x.DiscountPercent,
                        SortNo = x.SortNo,
                        BeginDate = x.BeginDate,
                        EndDate = x.EndDate,
                        DateRange = x.DateRange,
                    })
                    .FirstOrDefault<TourPriceBreakdown>(x => (x.EndPoint1 <= totalTravelers && totalTravelers <= x.EndPoint2)),
                    SubTotalPrice = (float)currBooking.SubTotal,
                    ServiceFee = (float)currBooking.ServiceFee,
                    Taxes = (int)currBooking.Taxes,
                    IfShowPromoCodeBox = false,
                    PromoPrice = (float)currBooking.PromoPrice,
                    TotalPrice = (float)currBooking.TotalPay,
                    DiscountTourists = currBooking.DiscountTourist,
                    DiscountPercent = (float)currBooking.DiscountPercent,
                    DiscountValue = (int)currBooking.DiscountValue,
                    MinTouristNum = tourObject.MinTouristNum,
                    MaxTouristNum = tourObject.MaxTouristNum,
                };
                //model.SubTotalPrice=((float)(tourScheduleObject.SugRetailPrice??0)
                //    +model.ExtraPrices1+model.ExtraPrices2
                //    +model.ExtraPrices3+model.ExtraPrices4
                //    +model.ExtraPrices5)*model.TotalTravelers;
                //if(model.DiscountValue>0)
                //{
                //    model.SubTotalPrice -= model.DiscountValue * model.TotalTravelers;
                //}
                //else if (model.DiscountPercent > 0)
                //{
                //    model.DiscountValue = (int)(model.SubTotalPrice * model.DiscountPercent/100);
                //    model.SubTotalPrice -= model.DiscountValue;
                //}
                //else
                //{
                //    model.DiscountValue = 0;
                //}
                //model.ServiceFee = (float)(model.SubTotalPrice * tourScheduleObject.CommisionPay / 100);
                //model.TotalPrice = model.SubTotalPrice - model.ServiceFee;
                //model.Taxes = 0;
                return View("ReceivedBookingDetails", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult SchedulerPrice(Nullable<int> id)
        {
            try
            {
                return _guideService.ShowSchedulerPrice(id);
                //TourSchedule tourSchedule = null;
                //List<TourExtra> tourExtras = null;
                //int tourExtrasSize = 0;
                //if (id != null)
                //{
                //    tourSchedule = _repositoryGuides.TourScheduleGetByTourID(id.Value);
                //    tourExtras = _repositoryGuides.TourExtraGetAllByTourID(id.Value).ToList();
                //    if (tourExtras != null)
                //    {
                //        tourExtrasSize = tourExtras.Count;
                //    }
                //}
                //var model = new SchedulerPriceModel
                //{
                //    IsMember = false,
                //    IsSignedIn = false,
                //    SelectedPage = LayoutSelectedPage.Account,

                //    TourID = id??0,
                //    ScheduleName = tourSchedule == null ? string.Empty : tourSchedule.Name,
                //    //BgnDate = tourSchedule == null ? DateTime.UtcNow.ToString(ConstantData.CommonTimeFormat) : tourSchedule.BeginDate.ToString(ConstantData.CommonTimeFormat),
                //    //EndDate = tourSchedule == null ? DateTime.UtcNow.ToString(ConstantData.CommonTimeFormat) : tourSchedule.EndDate.ToString(ConstantData.CommonTimeFormat),

                //    //Daterange = tourSchedule == null ? string.Empty : tourSchedule.DateRange,
                //    //IsMonday = tourSchedule == null ? false : tourSchedule.IsMonday,
                //    //IsTuesday = tourSchedule == null ? false : tourSchedule.IsTuesday,
                //    //IsWednesday = tourSchedule == null ? false : tourSchedule.IsWednesday,
                //    //IsThursday = tourSchedule == null ? false : tourSchedule.IsThursday,
                //    //IsFriday = tourSchedule == null ? false : tourSchedule.IsFriday,
                //    //IsSaturday = tourSchedule == null ? false : tourSchedule.IsSaturday,
                //    //IsSunday = tourSchedule == null ? false : tourSchedule.IsSunday,

                //    Daterange = tourSchedule == 
                //            null ? 
                //            string.Format("{0} - {1}",
                //            DateTime.Now.ToString(TimeHelper.DefaultDateFormat),
                //            DateTime.Now.AddYears(2).ToString(TimeHelper.DefaultDateFormat)) 
                //            : 
                //            tourSchedule.DateRange,
                //    IsMonday = tourSchedule == null ? true : tourSchedule.IsMonday,
                //    IsTuesday = tourSchedule == null ? true : tourSchedule.IsTuesday,
                //    IsWednesday = tourSchedule == null ? true : tourSchedule.IsWednesday,
                //    IsThursday = tourSchedule == null ? true : tourSchedule.IsThursday,
                //    IsFriday = tourSchedule == null ? true : tourSchedule.IsFriday,
                //    IsSaturday = tourSchedule == null ? true : tourSchedule.IsSaturday,
                //    IsSunday = tourSchedule == null ? true : tourSchedule.IsSunday,

                //    ExtraNames1 = tourExtrasSize>0 ? tourExtras[0].Name:string.Empty,
                //    ExtraPrices1 = tourExtrasSize > 0 ? tourExtras[0].Price : 0,
                //    ExtraTimes1 = tourExtrasSize > 0 ? tourExtras[0].Time : 0, 
                //    ExtraNames2 = tourExtrasSize > 1 ? tourExtras[1].Name : string.Empty,
                //    ExtraPrices2 = tourExtrasSize > 1 ? tourExtras[1].Price : 0,
                //    ExtraTimes2 = tourExtrasSize > 1 ? tourExtras[1].Time : 0,
                //    ExtraNames3 = tourExtrasSize > 2 ? tourExtras[2].Name : string.Empty,
                //    ExtraPrices3 = tourExtrasSize > 2 ? tourExtras[2].Price : 0,
                //    ExtraTimes3 = tourExtrasSize > 2 ? tourExtras[2].Time : 0,
                //    ExtraNames4 = tourExtrasSize > 3 ? tourExtras[3].Name : string.Empty,
                //    ExtraPrices4 = tourExtrasSize > 3 ? tourExtras[3].Price : 0,
                //    ExtraTimes4 = tourExtrasSize > 3 ? tourExtras[3].Time : 0,
                //    ExtraNames5 = tourExtrasSize > 4 ? tourExtras[4].Name : string.Empty,
                //    ExtraPrices5 = tourExtrasSize > 4 ? tourExtras[4].Price : 0,
                //    ExtraTimes5 = tourExtrasSize > 4 ? tourExtras[4].Time : 0,
             
                //    ExtraTimesType1 = tourExtrasSize > 0 ? tourExtras[0].TimeType:(int)TourTimeType.Days,
                //    ExtraTimesType2 = tourExtrasSize > 1 ? tourExtras[1].TimeType:(int)TourTimeType.Days,
                //    ExtraTimesType3 = tourExtrasSize > 2 ? tourExtras[2].TimeType:(int)TourTimeType.Days,
                //    ExtraTimesType4 = tourExtrasSize > 3 ? tourExtras[3].TimeType:(int)TourTimeType.Days,
                //    ExtraTimesType5 = tourExtrasSize > 4 ? tourExtras[4].TimeType:(int)TourTimeType.Days,
               
                //    TimesTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString()),
                
                //    DiscountTourists = tourSchedule == null ? 0 : tourSchedule.DiscountTourists??0,
                //    DiscountValue = tourSchedule == null ? 0 : tourSchedule.DiscountValue ?? 0,
                //    DiscountPercent = tourSchedule == null ? 0 : (float)(tourSchedule.DiscountPercent ?? 0),
                //    StartTime1 = tourSchedule == null ? "8:00am": tourSchedule.StartTime1,
                //    StartTime2 = tourSchedule == null ? string.Empty : tourSchedule.StartTime2,
                //    StartTime3 = tourSchedule == null ? string.Empty : tourSchedule.StartTime3,
                //    StartTime4 = tourSchedule == null ? string.Empty : tourSchedule.StartTime4,
                //    StartTime5 = tourSchedule == null ? string.Empty : tourSchedule.StartTime5,
                //    StartTime6 = tourSchedule == null ? string.Empty : tourSchedule.StartTime6,

                //    NetPrice = tourSchedule == null ? 0 : (float)(tourSchedule.NetPrice ?? 0),
                //    SugRetailPrice = tourSchedule == null ? 0 : (float)(tourSchedule.SugRetailPrice ?? 0),
                //    CommisionPay = tourSchedule == null ? 10 : (float)(tourSchedule.CommisionPay ?? 0),
                //    ID=tourSchedule == null ? 0 :tourSchedule.ID,
                //};
                //return View("SchedulerPrice", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public ActionResult SchedulerPrice(SchedulerPriceModel model)
        {
            try
            {
                ValidSchedulerPrice(model);
                if (ModelState.IsValid)
                {
                    return _guideService.SaveSchedulerPrice(model);
                    //var tourschedule = _repositoryGuides.TourScheduleGetByTourID(model.TourID);
                    //if (tourschedule == null)
                    //    tourschedule = new TourSchedule();
                    //else
                    //{
                    //    tourschedule.ModifyTime = DateTime.UtcNow;
                    //}
                    //tourschedule.TourID = model.TourID;
                    //tourschedule.Name = model.ScheduleName;
                    //tourschedule.DateRange = model.Daterange;
                    //DateTime start, end;
                    //TimeHelper.ConvertDateRangeStringToDateTime(model.Daterange,out start,out end);
                    //tourschedule.BeginDate=start;
                    //tourschedule.EndDate=end;
                    //tourschedule.IsMonday = model.IsMonday;
                    //tourschedule.IsTuesday = model.IsTuesday;
                    //tourschedule.IsWednesday = model.IsWednesday;
                    //tourschedule.IsThursday = model.IsThursday;
                    //tourschedule.IsFriday = model.IsFriday;
                    //tourschedule.IsSaturday = model.IsSaturday;
                    //tourschedule.IsSunday = model.IsSunday;
                    //tourschedule.DiscountTourists = model.DiscountTourists;
                    //tourschedule.DiscountValue = model.DiscountValue;
                    //tourschedule.DiscountPercent = model.DiscountPercent;
                    //tourschedule.StartTime1 = model.StartTime1;
                    //tourschedule.StartTime2 = model.StartTime2;
                    //tourschedule.StartTime3 = model.StartTime3;
                    //tourschedule.StartTime4 = model.StartTime4;
                    //tourschedule.StartTime5 = model.StartTime5;
                    //tourschedule.StartTime6 = model.StartTime6;
                    //tourschedule.NetPrice = model.NetPrice;
                    //tourschedule.SugRetailPrice = model.SugRetailPrice;
                    //tourschedule.CommisionPay = model.CommisionPay;

                    //_repositoryGuides.TourScheduleCreateOrUpdate(tourschedule);

                    //var extraNames = new List<string>() { model.ExtraNames1, model.ExtraNames2, 
                    //    model.ExtraNames3, model.ExtraNames4, model.ExtraNames5 };
                    //var extraPrices = new List<int>() { model.ExtraPrices1, model.ExtraPrices2, 
                    //    model.ExtraPrices3, model.ExtraPrices4, model.ExtraPrices5 };
                    //var extraTimes = new List<int>() { model.ExtraTimes1, model.ExtraTimes2, 
                    //    model.ExtraTimes3, model.ExtraTimes4, model.ExtraTimes5 };
                    //var extraTimesTypes = new List<int>() { model.ExtraTimesType1, model.ExtraTimesType2, 
                    //    model.ExtraTimesType3, model.ExtraTimesType4, model.ExtraTimesType5 };
                    //for (byte i = 0; i < extraNames.Count; i++)
                    //{
                    //    CheckAndSaveTourExtra(model.TourID, (byte)(i + 1), extraNames[i], extraPrices[i], extraTimes[i], (byte)extraTimesTypes[i]);
                    //}         

                    //return RedirectToAction("Pictures", new { id = model.TourID });                    
                }
                model.TimesTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString());
                return View("SchedulerPrice", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidSchedulerPrice(SchedulerPriceModel model)
        {
            if ((model.ExtraPrices1 != 0 || model.ExtraTimes1 != 0) && string.IsNullOrEmpty(model.ExtraNames1))
            {
                ModelState.AddModelError("ExtraNames1", string.Format(ValidationStrings.Required, "ExtraNames1"));          
            }
            if ((model.ExtraPrices2 != 0 || model.ExtraTimes2 != 0) && string.IsNullOrEmpty(model.ExtraNames2))
            {
                ModelState.AddModelError("ExtraNames2", string.Format(ValidationStrings.Required, "ExtraNames2"));
            }
            if ((model.ExtraPrices3 != 0 || model.ExtraTimes3 != 0) && string.IsNullOrEmpty(model.ExtraNames3))
            {
                ModelState.AddModelError("ExtraNames3", string.Format(ValidationStrings.Required, "ExtraNames3"));
            }
            if ((model.ExtraPrices4 != 0 || model.ExtraTimes4 != 0) && string.IsNullOrEmpty(model.ExtraNames4))
            {
                ModelState.AddModelError("ExtraNames4", string.Format(ValidationStrings.Required, "ExtraNames4"));
            }
            if ((model.ExtraPrices5 != 0 || model.ExtraTimes5 != 0) && string.IsNullOrEmpty(model.ExtraNames5))
            {
                ModelState.AddModelError("ExtraNames5", string.Format(ValidationStrings.Required, "ExtraNames5"));
            }
            if ((model.NetPrice / 0.9) - model.SugRetailPrice > 0)
            {
                ModelState.AddModelError("SugRetailPriceTooSmall", GuideStrings.SugRetailPriceTooSmall);
            }

            var breakdownsExtra = model.TourPriceBreakdownsExtra;
            var tour = _repositoryGuides.TourGetByID(model.ID);


            List<TourPriceBreakdownModel> allTourPriceBreakdowns = _guideService.ConcatLists<TourPriceBreakdownModel>(model.TourPriceBreakdowns, model.TourPriceBreakdownsExtra);
            var tourPriceBreakdownsCount = model.TourPriceBreakdowns==null ? 0 : model.TourPriceBreakdowns.Count;
            for (int i = 0; i < allTourPriceBreakdowns.Count; i++)
            {
                if (allTourPriceBreakdowns[i]?.DiscountValue==0 && allTourPriceBreakdowns[i]?.DiscountPercent==0)
                    continue;
                if (string.IsNullOrWhiteSpace(allTourPriceBreakdowns[i]?.DiscountValue?.ToString()) || string.IsNullOrWhiteSpace(allTourPriceBreakdowns[i]?.DiscountPercent?.ToString()))
                    continue;

                //if ((breakdownsExtra[i].EndPoint1<tour.MinTouristNum) || (breakdownsExtra[i].EndPoint2 > tour.MaxTouristNum))
                //{
                //    ModelState.AddModelError("TourPriceBreakdownExtraValidationMessage" + i, "");
                //}
                for (int j = 0; j < i; j++)
                {
                    if (allTourPriceBreakdowns[j]?.DiscountValue == 0 && allTourPriceBreakdowns[j]?.DiscountPercent == 0)
                        continue;
                    if (string.IsNullOrWhiteSpace(allTourPriceBreakdowns[j]?.DiscountValue?.ToString()) || string.IsNullOrWhiteSpace(allTourPriceBreakdowns[j]?.DiscountPercent?.ToString()))
                        continue;
                    if ((allTourPriceBreakdowns[i].EndPoint1 <= allTourPriceBreakdowns[j].EndPoint2) && (allTourPriceBreakdowns[i].EndPoint2 >= allTourPriceBreakdowns[j].EndPoint1))
                    {
                        if (i < tourPriceBreakdownsCount)
                        {
                            ModelState.AddModelError("TourPriceBreakdownValidationMessage" + i, GuideStrings.TourPriceBreakdownValidationMessage + " " + (j + 1));
                            return;
                        }
                        else
                        {
                            ModelState.AddModelError("TourPriceBreakdownExtraValidationMessage" + (i-tourPriceBreakdownsCount), GuideStrings.TourPriceBreakdownExtraValidationMessage + " " + (j+1));
                            return;
                        }
                    }
                }
            }

            List<TourVendorPromoModel> allTourVendorPromos = _guideService.ConcatLists<TourVendorPromoModel>(model.TourVendorPromos, model.TourVendorPromosExtra);
            var tourVendorPromosCount = model.TourVendorPromos == null ? 0 : model.TourVendorPromos.Count;
            for (int i = 0; i < allTourVendorPromos.Count; i++)
            {
                if (allTourVendorPromos[i]?.PromoValue == 0 && allTourVendorPromos[i]?.PromoPercent == 0)
                    continue;
                if (string.IsNullOrWhiteSpace(allTourVendorPromos[i]?.PromoValue.ToString()) || string.IsNullOrWhiteSpace(allTourVendorPromos[i]?.PromoPercent.ToString()))
                    continue;

                DateTime start_i, end_i;
                TimeHelper.ConvertDateRangeStringToDateTime(allTourVendorPromos[i].DateRange, out start_i, out end_i);

                for (int j = 0; j < i; j++)
                {
                    if (allTourVendorPromos[j]?.PromoValue == 0 && allTourVendorPromos[j]?.PromoPercent == 0)
                        continue;
                    if (string.IsNullOrWhiteSpace(allTourVendorPromos[j]?.PromoValue.ToString()) || string.IsNullOrWhiteSpace(allTourVendorPromos[j]?.PromoPercent.ToString()))
                        continue;
                    DateTime start_j, end_j;
                    TimeHelper.ConvertDateRangeStringToDateTime(allTourVendorPromos[j].DateRange, out start_j, out end_j);

                    if ((start_i <= end_j) && (end_i >= start_j))
                    {
                        if (i < tourPriceBreakdownsCount)
                        {
                            ModelState.AddModelError("TourVendorPromoValidationMessage" + i, GuideStrings.TourVendorPromoValidationMessage + " " + (j + 1));
                            return;
                        }
                        else
                        {
                            ModelState.AddModelError("TourVendorPromoExtraValidationMessage" + (i - tourVendorPromosCount), GuideStrings.TourVendorPromoExtraValidationMessage + " " + (j + 1));
                            return;
                        }
                        //ModelState.AddModelError("TourVendorPromoExtraValidationMessage" + i, GuideStrings.TourVendorPromoExtraValidationMessage + " " + (j + 1));
                    }
                }
            }
            return;
        }
        private void CheckAndSaveTourExtra(int tourId, byte sortNo, string name, int price,int time, byte timetype)
        {
            //if (!string.IsNullOrEmpty(name))
            //{
                var tourExtra = _repositoryGuides.TourExtraGetByTourIDSortNo(tourId, sortNo);
                if (tourExtra == null)
                {
                    tourExtra = new TourExtra()
                    {
                        Name = name??string.Empty,
                        Price = price,
                        Time = time,
                        TimeType = timetype,
                        SortNo = sortNo,
                        TourID = tourId,
                        EnterTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow,
                    };
                }
                else
                {
                    tourExtra.Name = name ?? string.Empty;
                    tourExtra.Price = price;
                    tourExtra.Time = time;
                    tourExtra.TimeType = timetype;
                    tourExtra.ModifyTime = DateTime.UtcNow;
                }
                _repositoryGuides.TourExtraCreateOrUpdate(tourExtra);
            //}
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult Pictures(Nullable<int> id)
        {
            return _guideService.ShowPictures(id);
            //List<TourPicture> tourPictures = null;
            //if (id != null)
            //{
            //    tourPictures = _repositoryGuides.TourPictureGetAllByTourID(id.Value).ToList();
            //}
            //var model = new PicturesModel
            //{
            //    IsMember = false,
            //    IsSignedIn = false,
            //    SelectedPage = LayoutSelectedPage.Account,
            //    TourID = id ?? 0,
            //    Pictures = tourPictures.Select(x => new SinglePicture()
            //    {
            //        Url = Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.RelativePath),
            //        ID=x.ID,
            //        TourID=x.TourID,
            //    }).ToList()
            //};
            //return View("Pictures", InitLayout(model));
        }
        [HttpPost]
        public ActionResult Pictures(PicturesModel model)
        {
            try
            {
                ValidPictures(model);
                if (ModelState.IsValid)
                {
                    return _guideService.SavePictures(model);
                    //_repositoryGuides.TourUpdateStatus(model.TourID, (byte)TourStatus.Complete);
                    //return RedirectToAction("TourProducts");
                }
                return View("Pictures", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidPictures(PicturesModel model)
        {
            var tourPictures = _repositoryGuides.TourPictureGetAllByTourID(model.TourID).ToList();
            if (tourPictures.Count<3)
                ModelState.AddModelError("ValidateInfo","Please upload at least 3 pictures for your tour");
            return;
        }
        [HttpPost]
        public JsonResult DeletePicture(int picId,int tourId)
        {
            try
            {
                var tourPicture = _repositoryGuides.TourPictureGetByID(picId);
                if (tourPicture == null)
                {
                    return Json(new
                    {
                        result = _ContentResultFailed,
                        data = "NOT FOUND!"
                    }, JsonRequestBehavior.DenyGet);                
                }
                FileSystemHelper.CheckAndDeleteOldFile(StaticSiteConfiguration.ImageFileDirectory, tourPicture.RelativePath, _logger);
                _repositoryGuides.TourPictureDeleteByPictureId(picId);
                return Json(new
                {
                    Result = true,
                    Data = Url.Action("Pictures", new { id = tourId }),
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Publish(Nullable<int> id)
        {
            return _guideService.ShowMiscellaneous(id);
        }
        [HttpPost]
        public ActionResult Publish(MiscellaneousModel model)
        {
            try
            {
                _guideService.ValidMiscellaneous(model);
                if (ModelState.IsValid)
                {
                    //_guideService.SaveMiscellaneous(model);
                    if (model.IfActive)
                    {
                        //_repositoryGuides.TourUpdateStatus(model.TourID, (byte)TourStatus.Active);
                        return RedirectToAction("TourSetStatus", "Guide",
                            new
                            {
                                id = model.TourID,
                                status = (byte) TourStatus.Active,
                                NextActionName = "TourProducts",
                                NextControllerName = RouteData.Values["controller"],
                                
                                DoneActionName = string.Format("Publish/{0}", model.TourID),
                                DoneControllerName = RouteData.Values["controller"]
                            });
                    }
                    else
                    {
                        return RedirectToAction("TourSetStatus", "Guide", 
                            new
                            {
                                id = model.TourID,
                                status = (byte)TourStatus.Complete,
                                NextActionName = "TourProducts",
                                NextControllerName = RouteData.Values["controller"],

                                DoneActionName = string.Format("Publish/{0}", model.TourID),
                                DoneControllerName = RouteData.Values["controller"]
                            });
                    }
                    //return RedirectToAction("TourProducts");
                }
                return View("Publish", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        //public ActionResult VerifyGuiderContactInfo(User user, bool verifyPhone)
        //{
        //    if ((user.OpenSite == 0 && !user.IsConfirmed) || (user.OpenSite > 0 && !user.IsEmailVerified4OpenID))
        //    {
        //        return RedirectToAction("ConfirmEmail", "Account");
        //    }
        //    if (!verifyPhone)
        //        return null;
        //    if (!user.IsPhoneConfirmed)
        //    {
        //        return RedirectToAction("ConfirmPhone", "Account");
        //    }
        //    return null;
        //}

        //[DonutOutputCache(CacheProfile = "OneDay")]
        [Authorize]
        public ActionResult TourProducts(Nullable<int> page)
        {
            var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
            if (user.RealRole != UserRole.Guider)
            {
                return RedirectToAction("Index", "Tourist");
            }
            DoneRedirect redirect = new DoneRedirect()
            {
                DoneControllerName = "Guide",
                DoneActionName = "TourProducts"
            };
            var result = _guideService.VerifyGuiderContactInfo(user, false, redirect);
            if (result != null)
                return result;
            
            var tours= _repositoryGuides.TourGetAllByPage(
                    _security.GetCurrentUserId(),
                    page == null ? 1 : page.Value,
                    _ListViewPageSizeBig)
                    .Select(x => new SimpleTourModel() { 
                        TourID=x.ID,
                        TourName=x.Name,
                        Status=x.RealStatus
                    }).ToList();
            tours.ForEach(tour=>{
                var tourId = tour.TourID;
                var firstPic = _repositoryGuides.TourPictureGetAllByTourID(tourId).OrderBy(x=>x.SortNo).Take(1).SingleOrDefault();
                if (firstPic == null)
                    return;
                tour.TourImageUrl = Path.Combine(StaticSiteConfiguration.ImageServerUrl, firstPic.RelativePath);

                //Check if the tour already expires or not
                DateTime endDate = (DateTime)_repositoryGuides.TourScheduleGetByTourID(tourId).EndDate;
                if (endDate == null)
                    return;
                tour.IfExpired = endDate < DateTime.Now;
            });

            var model = new TourProductsModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                Tours = tours,
            };
            return View("TourProducts", InitLayout(model));

        }
        [HttpPost]
        public ActionResult TourProducts(TourProductsModel model)
        {
            try
            {
                ValidTourProducts(model);
                if (ModelState.IsValid)
                {
                    return RedirectToAction("TourProducts");
                }
                return View("TourProducts", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidTourProducts(TourProductsModel model)
        {

            return;
        }
        public ActionResult TourDelete(int id)
        {
            try
            {
                _repositoryGuides.TourDeleteByID(id);
                return RedirectToAction("TourProducts");
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        public ActionResult TourClone(int id)
        {
            try
            {
                var newTour=_repositoryGuides.TourCloneByID(id);
                return RedirectToAction("TourType", new { id=newTour.ID });
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        //public ActionResult TourSetStatus(int id, byte status, string nextActionName, string nextControllerName, DoneRedirect redirect)
        public ActionResult TourSetStatus(int id, byte status, string nextActionName, string nextControllerName, string doneActionName, string doneControllerName)
        {
            try
            {
                if (status == (byte)TourStatus.Active)
                {
                    var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());

                    DoneRedirect redirect = new DoneRedirect()
                    {
                        DoneActionName = doneActionName,
                        DoneControllerName = doneControllerName
                    };
                    var result = _guideService.VerifyGuiderContactInfo(user, true, redirect);
                    if (result != null)
                        return result;
                    var tour = _repositoryGuides.TourGetByID(id);
                    var city = _repositoryCities.GetCityByIdInDB(tour.TourCityID);
                    if (city != null && city.IsInUse != null && !city.IsInUse.Value)
                    {
                        city.IsInUse = true;
                        _repositoryCities.CityUpdateIsInUseInfo(city);
                    }
                }

                //Beahvior Change: Now, allow booked tour to be deactivated(not visible to the public).
                //else
                //{
                //    int bookCount = _repositoryGuides.UserTourBookingGetCountByTourID(id);
                //    if (bookCount > 0)
                //    {
                //        //return RedirectToAction("Message", "Notifications", new {title = "Tour already booked",text="The tour status can NOT be changed as somebody has booked your tour." });    
                //        return RedirectToAction("WarningToEdit");
                //    }
                //}

                _repositoryGuides.TourUpdateStatus(id, status); //Actual Status Update

                //string actionName = redirect?.NextActionName ?? (string)RouteData.Values["action"];
                //return RedirectToAction(actionName);

                //if (redirect == null)
                //{
                //    RedirectToAction((string)RouteData.Values["action"]);
                //}
                return RedirectToAction(nextActionName, nextControllerName);

            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        
        //public ActionResult TourSetStatus(int id, byte status, DoneRedirect redirect)
        //{
        //    try
        //    {
        //        if (status == (byte)TourStatus.Active)
        //        {
        //            var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
        //            var result = _guideService.VerifyGuiderContactInfo(user, true, redirect);
        //            if (result != null)
        //                return result;
        //            var tour = _repositoryGuides.TourGetByID(id);
        //            var city = _repositoryCities.GetCityByIdInDB(tour.TourCityID);
        //            if (city != null && city.IsInUse != null && !city.IsInUse.Value)
        //            {
        //                city.IsInUse = true;
        //                _repositoryCities.CityUpdateIsInUseInfo(city);
        //            }
        //        }
        //        //Beahvior Change: Now, allow booked tour to be deactivated(not visible to the public).
        //        //else
        //        //{
        //        //    int bookCount = _repositoryGuides.UserTourBookingGetCountByTourID(id);
        //        //    if (bookCount > 0)
        //        //    {
        //        //        //return RedirectToAction("Message", "Notifications", new {title = "Tour already booked",text="The tour status can NOT be changed as somebody has booked your tour." });    
        //        //        return RedirectToAction("WarningToEdit");
        //        //    }
        //        //}
        //        _repositoryGuides.TourUpdateStatus(id, status); //Actual Status Update

        //        //string actionName = redirect?.NextActionName ?? (string)RouteData.Values["action"];
        //        //return RedirectToAction(actionName);

        //        //if (redirect == null)
        //        //{
        //        //    RedirectToAction((string)RouteData.Values["action"]);
        //        //}

        //        return RedirectToAction(redirect.NextActionName);
                
        //    }
        //    catch (Exception excp)
        //    {
        //        _logger.LogError(excp);
        //        return RedirectToAction("InternalError", "Static", null);
        //    }
        //}

        //public ActionResult TourSetStatus(int id, byte status)
        //{
        //    try
        //    {
        //        if (status == (byte)TourStatus.Active)
        //        {
        //            var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
        //            var result = _guideService.VerifyGuiderContactInfo(user, true);
        //            if (result != null)
        //                return result;
        //            var tour = _repositoryGuides.TourGetByID(id);
        //            var city = _repositoryCities.GetCityByIdInDB(tour.TourCityID);
        //            if (city != null && city.IsInUse != null && !city.IsInUse.Value)
        //            {
        //                city.IsInUse = true;
        //                _repositoryCities.CityUpdateIsInUseInfo(city);
        //            }
        //        }
        //        else
        //        {
        //            int bookCount = _repositoryGuides.UserTourBookingGetCountByTourID(id);
        //            if (bookCount > 0)
        //            {
        //                //return RedirectToAction("Message", "Notifications", new {title = "Tour already booked",text="The tour status can NOT be changed as somebody has booked your tour." });    
        //                return RedirectToAction("WarningToEdit");
        //            }
        //        }
        //        _repositoryGuides.TourUpdateStatus(id, status);
        //        return RedirectToAction("TourProducts");
        //    }
        //    catch (Exception excp)
        //    {
        //        _logger.LogError(excp);
        //        return RedirectToAction("InternalError", "Static", null);
        //    }
        //}

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult BankInformation()
        {
            var bankInfo = _repositoryGuides.BankInfoGetByUserID(_security.GetCurrentUserId());

            var model = new BankInformationModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                BankName = bankInfo==null?string.Empty:bankInfo.Name,
                BankBranch = bankInfo == null ? string.Empty : bankInfo.Branch,
                BankAccountNo = bankInfo == null ? string.Empty : bankInfo.AccountNo,
                BankAccountType = bankInfo == null ? string.Empty : bankInfo.AccountType,
                BankAccountOwner = bankInfo == null ? string.Empty : bankInfo.AccountOwner,
                ID = bankInfo == null ? 0 : bankInfo.ID,
            };
            return View("BankInformation", InitLayout(model));
        }
        [HttpPost]
        public ActionResult BankInformation(BankInformationModel model)
        {
            try
            {
                ValidBankInformation(model);
                if (ModelState.IsValid)
                {
                    var bankInfo = _repositoryGuides.BankInfoGetByUserID(_security.GetCurrentUserId());
                    if (bankInfo == null)
                    {
                        bankInfo = new BankInfo();
                        bankInfo.UserID = _security.GetCurrentUserId();
                    }
                    bankInfo.Name = model.BankName;
                    bankInfo.Branch = model.BankBranch;
                    bankInfo.AccountNo = model.BankAccountNo;
                    bankInfo.AccountType = model.BankAccountType;
                    bankInfo.AccountOwner = model.BankAccountOwner;
                    _repositoryGuides.BankInfoCreateOrUpdate(bankInfo);
                    return View("BankInformation", InitLayout(model));
                }
                return View("BankInformation", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidBankInformation(BankInformationModel model)
        {

            return;
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult Account()
        {
            try
            {
                var user =_repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                var model = new AccountModel
                {
                    IsMember = false,
                    IsSignedIn = false,
                    SelectedPage = LayoutSelectedPage.Account,
                    FirstName=user.FirstName,
                    LastName = user.LastName,
                    //OldPassword = user.Password,
                    EmailAddress = user.Email,
                    PhoneNumber = user.PhoneLocalCode,
                    IntroductionYourself = user.Bio,
                    AvatarUrl =string.IsNullOrEmpty(user.AvatarPath)?string.Empty:Path.Combine(StaticSiteConfiguration.ImageServerUrl, user.AvatarPath),
                    VideoUrl = string.IsNullOrEmpty(user.VideoPath) ? string.Empty : Path.Combine(StaticSiteConfiguration.ImageServerUrl, user.VideoPath),
                };
                return View("Account", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public ActionResult Account(AccountModel model)
        {
            try
            {
                ValidAccount(model);
                if (ModelState.IsValid)
                {
                    var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.EmailAddress;
                    user.PhoneLocalCode = model.PhoneNumber;
                    user.Bio = model.IntroductionYourself;
                    user.ModifyTime = DateTime.UtcNow;
                    _repositoryUsers.UserUpdate(user);
                    return RedirectToAction("Account");
                }
                return View("Account", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidAccount(AccountModel model)
        {

            return;
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult ReceivedBookings(Nullable<int> id)
        {
            //Tour tour = null;
            //if (id != null)
            //{
            //    tour = _repositoryGuides.TourGetByID(id.Value);
            //}

            var model = new ReceivedBookingsModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,

                //Name = tour == null ? string.Empty : tour.Name,
                //ID = tour == null ? 0 : tour.ID,
            };
            return View("ReceivedBookings", InitLayout(model));
        }

        [HttpPost]
        public ActionResult ReceivedBookings(ReceivedBookingsModel model)
        {
            try
            {
                ValidReceivedBookings(model);
                if (ModelState.IsValid)
                {
                    return RedirectToAction("ReceivedBookings");
                }
                return View("ReceivedBookings", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidReceivedBookings(ReceivedBookingsModel model)
        {

            return;
        }        
        public JsonResult ReceivedBookData(string start,string end)
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                if (userId <= 0)
                    return Json(new
                    {
                        Result = false
                    }, JsonRequestBehavior.AllowGet);
                var startTime=DateTime.ParseExact(
                     start,
                     TimeHelper.FullCalendarDateFormat,
                     CultureInfo.InvariantCulture);
                var endTime = DateTime.ParseExact(
                     end,
                     TimeHelper.FullCalendarDateFormat,
                     CultureInfo.InvariantCulture);
                var bookings = _repositoryGuides.UserTourBookingGetAllByTourUserIDStartEnd(userId, startTime, endTime);
                var events = bookings.Select(x => new ReceivedBookingEvent()
                {
                    id = x.ID.ToString(),
                    title = x.TourName,
                    start = x.CalendarStart.ToString(TimeHelper.DefaultDateFormat),
                    end = x.CalendarEnd.ToString(TimeHelper.DefaultDateFormat),
                    url = Url.Action("ReceivedBookingDetails", new {id =x.ID,tourId=x.TourID,touristId=x.UserID}),
                    editable=false,
                    allDay=true,
                });
                return Json(events,JsonRequestBehavior.AllowGet);
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
            }
            return Json(new
            {
                Result = false
            }, JsonRequestBehavior.AllowGet);
        }



        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult ExcludedDates(Nullable<int> id)
        {
            var dates = _repositoryGuides.GuiderExcludedDatesGetByUserID(_security.GetCurrentUserId())
                .Select(x=>x.Date).ToList();
            var model = new ExcludedDatesModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                ExcludedDates = CreateJsonResponse(dates).Content,
                OrderedDates = CreateJsonResponse(new List<string>()).Content,
            };
            return View("ExcludedDates", InitLayout(model));

        }
        [HttpPost]
        public ActionResult ExcludedDates(ExcludedDatesModel model)
        {
            try
            {
                ValidExcludedDates(model);
                if (ModelState.IsValid)
                {
                    GuiderExcludedDates key = new GuiderExcludedDates()
                    {
                        UserID = model.UserID,
                        Date = model.ExcludedDates,
                    };
                    //_repositoryGuides.GuiderExcludedDatesCreateOrUpdate();
                    return RedirectToAction("ExcludedDates");
                }
                return View("ExcludedDates", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidExcludedDates(ExcludedDatesModel model)
        {

            return;
        }
        [HttpPost]
        public JsonResult SetExpireDate(int clickType, string clickDay)
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                var ged = _repositoryGuides.GuiderExcludedDatesGetByUserIDDate(userId, clickDay);
                if (ged == null)
                {
                    ged = new GuiderExcludedDates()
                    {
                        UserID = userId,
                        Date = clickDay,
                        Status = (byte)clickType,
                        EnterTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow,
                    };
                }
                else
                {
                    ged.Status = (byte)clickType;
                    ged.ModifyTime = DateTime.UtcNow;
                }
                _repositoryGuides.GuiderExcludedDatesCreateOrUpdate(ged);
                return Json(new
                {
                    result = true,
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }


        [HttpPost]
        public JsonResult UploadFiles(int? id)
        {
            try{
                int tourId =SafeConvert.ToInt32(Request.Form["tourId"]);
                int uploadType = SafeConvert.ToInt32(Request.Form["uploadType"]);
                if (uploadType==0&&tourId <= 0)
                {
                    return Json(new
                    {
                        result = false,
                        data = "Sorry,internal error!"
                    }, JsonRequestBehavior.DenyGet); 
                }
                string relativePath = string.Empty,fileExt=string.Empty, path=string.Empty,redirectPath=string.Empty;
                HttpPostedFileBase file;
                int total = 0;
                if (uploadType == 0)
                {
                    total = _repositoryGuides.TourPictureGetTotalCountOfTourId(tourId);
                    if (total >= 10)
                    {
                        return Json(new
                        {
                            result = false,
                            data = "Sorry,you can only upload 10 pictures in maximum for a tour!"
                        }, JsonRequestBehavior.DenyGet);                     
                    }
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    file = Request.Files[i];
                    fileExt = Path.GetExtension(file.FileName);
                    switch (uploadType)
                    { 
                        case 0:
                        default:
                            relativePath = FileSystemHelper.GenerateTourImagePath(fileExt);
                            _repositoryGuides.TourPictureCreateOrUpdate(new TourPicture() { 
                                 TourID=tourId,
                                 RelativePath=relativePath,
                                 SortNo = (byte)(total+i),
                                 EnterTime=DateTime.UtcNow,
                                 ModifyTime=DateTime.UtcNow,
                            });
                            redirectPath=Url.Action("Pictures", new { id = tourId });
                            break;
                        case 1://upload avatar
                            relativePath = FileSystemHelper.GenerateAvatarImagePath(fileExt);
                            if (id == null || IsNotAdmin())
                                _repositoryUsers.UserUpdateAvatar(_security.GetCurrentUserId(), relativePath);
                            else
                                _repositoryUsers.UserUpdateAvatar(id.Value, relativePath);
                            redirectPath = Url.Action("Account", new { id = tourId });
                            break;
                        case 2://upload video
                            relativePath = FileSystemHelper.GenerateVideoPath(fileExt);
                            _repositoryUsers.UserUpdateVideo(_security.GetCurrentUserId(), relativePath);
                            redirectPath = Url.Action("Account", new { id = tourId });
                            break;
                    }
                    path = Path.Combine(StaticSiteConfiguration.ImageFileDirectory, relativePath);
                    FileHelper.CheckDirectory(path);
                    file.SaveAs(path);

                }
                return Json(new
                {
                    result = true,
                    data = redirectPath,
                }, JsonRequestBehavior.DenyGet); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        [Route("Become-a-Guide", Order = 1)]
        [Route("Guide/Join", Order = 2)]
        public ActionResult Join()
        {
            return View("Join", InitLayout(new Layout() {
                IsIndex = true
            }));
        }

        public ActionResult WarningToEdit()
        {
            return View("WarningToEdit", InitLayout(new Layout()));
        }
        public double CalculateVendorPromotedPrice(double nowPrice, IEnumerable<TourVendorPromo> tourVendorPromoSubset)
        {
            foreach (var vendorPromo in tourVendorPromoSubset)
            {
                if (vendorPromo.BeginDate <= DateTime.Now && DateTime.Now <= vendorPromo.EndDate)
                {
                    var promoValue = vendorPromo.PromoValue;
                    var promoPercent = vendorPromo.PromoPercent;
                    if (promoValue > 0)
                    {
                        return nowPrice - promoValue;
                    }
                    if (promoPercent > 0)
                    {
                        return Math.Round((1 - promoPercent / 100) * nowPrice);
                    }
                }
            }

            return nowPrice;
        }

    }
}
