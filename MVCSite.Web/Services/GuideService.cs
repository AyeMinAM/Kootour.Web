using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Common;
using MVCSite.Common;
using MVCSite.ViewResource;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Extensions;
using MVCSite.DAC.Interfaces;
using MVCSite.Web.Controllers;
using NLog.Fluent;

namespace MVCSite.Web.Services
{
    public class GuideService : LayoutBase
    {
        private readonly IPublicCommands _commands;
        protected readonly IRepositoryGuides _repositoryGuides;
        protected readonly IRepositoryCities _repositoryCities;

        public GuideService(
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
        }

        public ActionResult ShowTourType(Nullable<int> id)
        {
            Tour tour = null;
            if (id != null)
            {
                tour = _repositoryGuides.TourGetByID(id.Value);
            }
            else
            {
                tour = _repositoryGuides.TourGetByUserIDStatus(_security.GetCurrentUserId(), TourStatus.Incomplete);
            }
            int[] lanIds = new int[] { (int)LanguageCode.English };
            if (tour != null)
            {
                var tourLans = new List<int>();
                bool[] lanFlags = new bool[] {
                    tour.IsEnglish,
                    tour.IsChineseMandarian,
                    tour.IsChineseCantonese,
                    tour.IsFrench,
                    tour.IsSpanish,
                    tour.IsGerman,
                    tour.IsPortuguese,
                    tour.IsItalian,
                    tour.IsRussian,
                    tour.IsKorean,
                    tour.IsJapanese,
                    tour.IsNorwegian,
                    tour.IsSwedish,
                    tour.IsDanish,
                };
                for (int i = 0; i < lanFlags.Length; i++)
                {
                    if (lanFlags[i])
                        tourLans.Add(i + 1);
                }
                lanIds = tourLans.ToArray();
            }
            var model = new TourTypeModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                Name = tour == null ? string.Empty : tour.Name,
                IsHistorical = tour == null ? false : tour.IsHistorical,
                IsAdventure = tour == null ? false : tour.IsAdventure,
                IsLeisureSports = tour == null ? false : tour.IsLeisureSports,
                IsCultureArts = tour == null ? false : tour.IsCultureArts,
                IsNatureRural = tour == null ? false : tour.IsNatureRural,
                IsFestivalEvents = tour == null ? false : tour.IsFestivalEvents,
                IsNightlifeParty = tour == null ? false : tour.IsNightlifeParty,
                IsFoodDrink = tour == null ? false : tour.IsFoodDrink,
                IsShoppingMarket = tour == null ? false : tour.IsShoppingMarket,
                IsTransportation = tour == null ? false : tour.IsTransportation,
                IsBusinessInterpretation = tour == null ? false : tour.IsBusinessInterpretation,
                IsPhotography = tour == null ? false : tour.IsPhotography,
                LanguageIDs = lanIds,
                LanguageOptions = LanguageTranslation.Translations.Skip(1).ToSelectList(x => x.Language, x => x.Code.ToString()),
                ID = tour == null ? 0 : tour.ID,
                GuideID = tour == null ? 0 : tour.UserID,
            };
            return View("TourType", InitLayout(model));
        }

        public ActionResult SaveTourType(TourTypeModel model)
        {
            var tour = _repositoryGuides.TourGetByID(model.ID);
            if (tour == null)
            {
                tour = new Tour();
                tour.UserID = _security.GetCurrentUserId();//model.GuideID;
            }
            
            tour.Name = model.Name;
            tour.IsHistorical = model.IsHistorical;
            tour.IsAdventure = model.IsAdventure;
            tour.IsLeisureSports = model.IsLeisureSports;
            tour.IsCultureArts = model.IsCultureArts;
            tour.IsNatureRural = model.IsNatureRural;
            tour.IsFestivalEvents = model.IsFestivalEvents;
            tour.IsNightlifeParty = model.IsNightlifeParty;
            tour.IsFoodDrink = model.IsFoodDrink;
            tour.IsShoppingMarket = model.IsShoppingMarket;
            tour.IsTransportation = model.IsTransportation;
            tour.IsBusinessInterpretation = model.IsBusinessInterpretation;
            tour.IsPhotography = model.IsPhotography;

            tour.IsEnglish = model.LanguageIDs.Contains((int)LanguageCode.English);
            tour.IsChineseMandarian = model.LanguageIDs.Contains((int)LanguageCode.ChineseMandarian);
            tour.IsChineseCantonese = model.LanguageIDs.Contains((int)LanguageCode.ChineseCantonese);
            tour.IsFrench = model.LanguageIDs.Contains((int)LanguageCode.French);
            tour.IsSpanish = model.LanguageIDs.Contains((int)LanguageCode.Spanish);
            tour.IsGerman = model.LanguageIDs.Contains((int)LanguageCode.German);
            tour.IsPortuguese = model.LanguageIDs.Contains((int)LanguageCode.Portuguese);
            tour.IsItalian = model.LanguageIDs.Contains((int)LanguageCode.Italian);
            tour.IsRussian = model.LanguageIDs.Contains((int)LanguageCode.Russian);
            tour.IsKorean = model.LanguageIDs.Contains((int)LanguageCode.Korean);
            tour.IsJapanese = model.LanguageIDs.Contains((int)LanguageCode.Japanese);
            tour.IsNorwegian = model.LanguageIDs.Contains((int)LanguageCode.Norwegian);
            tour.IsSwedish = model.LanguageIDs.Contains((int)LanguageCode.Swedish);
            tour.IsDanish = model.LanguageIDs.Contains((int)LanguageCode.Danish);
            tour.ModifyTime = DateTime.UtcNow;
            tour.IsFeatured = false;
            _repositoryGuides.TourCreateOrUpdate(tour);
            return RedirectToAction("Overview", new { id = tour.ID });
        }

        public ActionResult ShowOverview(Nullable<int> id)
        {
            Tour tour = null;
            List<TourInclusion> inclusions = null;
            List<TourExclusion> exclusions = null;

            List<TourInclusionExclusionModel> tourInclusions = new List<TourInclusionExclusionModel>();
            List<TourInclusionExclusionModel> tourExclusions = new List<TourInclusionExclusionModel>();
            List<TourInclusionExclusionModel> tourInclusionsExtra = new List<TourInclusionExclusionModel>();
            List<TourInclusionExclusionModel> tourExclusionsExtra = new List<TourInclusionExclusionModel>();
            TourInclusionExclusionModel emptyTourInclusionExclusion = new TourInclusionExclusionModel();
            for (int n = 0; n < 20; n++)
            {
                tourInclusionsExtra.Add(emptyTourInclusionExclusion);
                tourExclusionsExtra.Add(emptyTourInclusionExclusion);
            }
            

            if (id != null)
            {
                tour = _repositoryGuides.TourGetByID(id.Value);
                inclusions = _repositoryGuides.TourInclusionGetAllByTourID(id.Value).OrderBy(x => x.SortNo).ToList();
                exclusions = _repositoryGuides.TourExclusionGetAllByTourID(id.Value).OrderBy(x => x.SortNo).ToList();
            }
            int inclusionCount = inclusions != null ? inclusions.Count : 0;
            int exclusionCount = exclusions != null ? exclusions.Count : 0;

            inclusions?.ForEach(element => tourInclusions.Add(new TourInclusionExclusionModel(element)));
            exclusions?.ForEach(element => tourExclusions.Add(new TourInclusionExclusionModel(element)));

            string cityString = string.Empty;
            if (tour != null)
            {
                var city = _repositoryCities.GetCityByIdInDB(tour.TourCityID);
                if (city != null)
                {
                    cityString = city.UniqueCityName;
                }
            }
            var model = new TourOverviewModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                Overview = tour == null ? string.Empty : tour.Overview,
                Itinerary = tour == null ? string.Empty : tour.Itinerary,
                Duration = tour == null ? 0 : tour.Duration,
                DurationType = tour == null ? (int)TourTimeType.Days : tour.DurationTimeType,
                DurationTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString()),
                //TourCityID = tour == null ? 0 : tour.TourCityID,
                //CityOptions=_repositoryCities.GetCitiesSelectListItemCached(),
                TourCity = cityString,
                TourCityHidden = cityString,
                MeetupLocation = tour == null ? string.Empty : tour.MeetupLocation,

                TourInclusions = tourInclusions,
                TourInclusionsExtra = tourInclusionsExtra,
                TourExclusions = tourExclusions,
                TourExclusionsExtra = tourExclusionsExtra,

                ID = tour == null ? 0 : tour.ID,
                GuideID = tour == null ? 0 : tour.UserID,
            };
            return View("Overview", InitLayout(model));
        }

        //KW: 2016-12-01 This new class was added to hold code to be reused for Guide and 
        //Admin Controllers. The mechanism part of this class was originally from Guide 
        //Controller’s action method called “Overview”. I removed fixed 5 Inclusions and 
        //Exclusions properties from “TourOverviewModel”, and added a generic list to this 
        //ViewModel in order to hold undetermined number of Inclusions/Exclusions.
        public ActionResult SaveOverview(TourOverviewModel model)
        {
            var cityInUrl = string.Empty;
            City city = null;
            int cityId = 0;
            if (!string.IsNullOrEmpty(model.TourCityHidden))
            {
                cityInUrl = model.TourCityHidden.Replace(",", "-").ToLower();
                city = _repositoryCities.GetCityByNameInUrlOrNullInDB(cityInUrl);
                if (city == null)
                {
                    var parts = model.TourCityHidden.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts == null)
                    {
                        ModelState.AddModelError("TourCity", "Please input a valid city");
                    }
                    else
                    {
                        int partLen = parts.Length;
                        if (partLen != 3)
                        {
                            ModelState.AddModelError("TourCity", "Please input a valid city");
                        }
                        else
                        {
                            var country = _repositoryCities.CountryCreateOrGet(parts[2]);
                            var region = _repositoryCities.RegionCreateOrGet(parts[1], country.country_id);
                            city = _repositoryCities.CityCreateOrGet(parts[0], model.TourCityHidden,
                                cityInUrl, country.country_id, region.Id);
                            cityId = city.CityId;
                        }
                    }
                }
                else
                {
                    cityId = city.CityId;
                }
            }
            var tour = _repositoryGuides.TourGetByID(model.ID);
            if (tour == null)
            {
                tour = new Tour();
            }
            tour.UserID = model.GuideID;//_security.GetCurrentUserId();
            tour.Overview = model.Overview;
            tour.Itinerary = model.Itinerary;
            tour.Duration = model.Duration;
            tour.DurationTimeType = (byte)model.DurationType;
            tour.TourCityID = cityId;
            tour.MeetupLocation = model.MeetupLocation;
            tour.ModifyTime = DateTime.UtcNow;
            tour = _repositoryGuides.TourCreateOrUpdate(tour);

            //List<TourInclusionExclusionModel> allTourInclusions = null;
            //if (model.TourInclusions == null && model.TourInclusionsExtra == null) {
            //    allTourInclusions = new List<TourInclusionExclusionModel>();
            //}
            //else if (model.TourInclusions == null) {
            //    allTourInclusions = model.TourInclusionsExtra;
            //}
            //else if (model.TourInclusionsExtra == null) {
            //    allTourInclusions = model.TourInclusions;
            //}
            //else {
            //    allTourInclusions = model.TourInclusions.Concat(model.TourInclusionsExtra).ToList();
            //}
            List<TourInclusionExclusionModel> allTourInclusions = ConcatLists(model.TourInclusions, model.TourInclusionsExtra);
            int inclusionCount = 0;
            for (int i = 0; i < allTourInclusions.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(allTourInclusions[i].Name))
                {
                    //CheckAndDeleteTourInclusion
                    continue;
                }
                CheckAndSaveTourInclusion(tour.ID, (byte)(inclusionCount + 1), allTourInclusions[i].Name);
                inclusionCount++;
            }
            //Delete old unused ones with SortNo > (byte)(inclusionCount + 1)
            DeleteOldTourInclusions(tourId: tour.ID, sortNo: (byte)(inclusionCount + 1));

            //List<TourInclusionExclusionModel> allTourExclusions = null;
            //if (model.TourExclusions == null && model.TourExclusionsExtra == null)
            //{
            //    allTourExclusions = new List<TourInclusionExclusionModel>();
            //}
            //else if (model.TourExclusions == null)
            //{
            //    allTourExclusions = model.TourExclusionsExtra;
            //}
            //else if (model.TourExclusionsExtra == null)
            //{
            //    allTourExclusions = model.TourExclusions;
            //}
            //else
            //{
            //    allTourExclusions = model.TourExclusions.Concat(model.TourExclusionsExtra).ToList();
            //}
            List<TourInclusionExclusionModel> allTourExclusions = ConcatLists<TourInclusionExclusionModel>(model.TourExclusions, model.TourExclusionsExtra);
            int exclusionCount = 0;
            for (int i = 0; i < allTourExclusions.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(allTourExclusions[i].Name))
                {
                    //CheckAndDeleteTourExclusion
                    continue;
                }
                CheckAndSaveTourExclusion(tour.ID, (byte)(exclusionCount + 1), allTourExclusions[i].Name);
                exclusionCount++;
            }
            //Delete old unused ones with SortNo > (byte)(exclusionCount + 1)
            DeleteOldTourExclusions(tourId: tour.ID, sortNo: (byte)(exclusionCount + 1));

            return RedirectToAction("BookingDetails", new { id = tour.ID });
        }
        //KW: 2016-12-01 End of change

        public ActionResult ShowBookingDetails(Nullable<int> id)
        {
            Tour tour = null;
            if (id != null)
            {
                tour = _repositoryGuides.TourGetByID(id.Value);
            }
            if (tour == null)
            {
                Log.Error("Tour #" + id.Value + " is not found in DB table.");
                return RedirectToAction("InternalError", "Static", null);
            }
            var model = new BookingDetailsModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                MinTouristNum = tour == null ? 0 : tour.MinTouristNum,
                MaxTouristNum = tour == null ? 0 : tour.MaxTouristNum,

                BookingType = tour == null ? (byte)0 : tour.BookingType,
                MinHourAdvance = tour == null ? 0 : tour.MinHourAdvance,
                ID = tour == null ? 0 : tour.ID,
                GuideID = tour == null ? 0 : tour.UserID,
            };
            return View("BookingDetails", InitLayout(model));
        }

        public ActionResult SaveBookingDetails(BookingDetailsModel model)
        {
            var tour = _repositoryGuides.TourGetByID(model.ID);
            if (tour == null)
            {
                tour = new Tour();
            }
            tour.UserID = model.GuideID;//_security.GetCurrentUserId();
            tour.MinTouristNum = model.MinTouristNum;
            tour.MaxTouristNum = model.MaxTouristNum;
            tour.BookingType = model.BookingType;
            tour.MinHourAdvance = model.MinHourAdvance;
            tour.ModifyTime = DateTime.UtcNow;
            _repositoryGuides.TourCreateOrUpdate(tour);
            return RedirectToAction("SchedulerPrice", new { id = tour.ID });
        }

        public ActionResult ShowSchedulerPrice(Nullable<int> id)
        {
            TourSchedule tourSchedule = null;
            List<TourExtra> tourExtras = null;
            List<TourPriceBreakdown> priceBreakdowns = null;
            List<TourPriceBreakdownModel> tourPriceBreakdowns = new List<TourPriceBreakdownModel>();
            List<TourPriceBreakdownModel> tourPriceBreakdownsExtra = new List<TourPriceBreakdownModel>();
            List<TourVendorPromo> vendorPromos = null;
            List<TourVendorPromoModel> tourVendorPromos = new List<TourVendorPromoModel>();
            List<TourVendorPromoModel> tourVendorPromosExtra = new List<TourVendorPromoModel>();

            var tour = _repositoryGuides.TourGetByID(id.Value);
            TourPriceBreakdownModel emptyTourPriceBreakdownsExtra = new TourPriceBreakdownModel()
            {
                EndPoint1 = tour.MinTouristNum,
                EndPoint2 = tour.MaxTouristNum,
                DiscountValue = 0,
                DiscountPercent = 0
            };
            for (int n = 0; n < 15; n++)
            {
                tourPriceBreakdownsExtra.Add(emptyTourPriceBreakdownsExtra);
            }



            int tourExtrasSize = 0;
            if (id != null)
            {
                tourSchedule = _repositoryGuides.TourScheduleGetByTourID(id.Value);
                tourExtras = _repositoryGuides.TourExtraGetAllByTourID(id.Value).ToList();
                if (tourExtras != null)
                {
                    tourExtrasSize = tourExtras.Count;
                }
                priceBreakdowns = _repositoryGuides.TourPriceBreakdownGetAllByTourID(id.Value).ToList();
                vendorPromos = _repositoryGuides.TourVendorPromoGetAllByTourID(id.Value).ToList();
            }
            priceBreakdowns?.ForEach(element => tourPriceBreakdowns.Add(new TourPriceBreakdownModel(element)));
            vendorPromos?.ForEach(element => tourVendorPromos.Add(new TourVendorPromoModel(element)));

            //TourVendorPromoModel emptyTourVendorPromosExtra = new TourVendorPromoModel()
            //{
            //    PromoValue = 0,
            //    PromoPercent = 0
            //};
            for (int n = 0; n < 5; n++)
            {
                tourVendorPromosExtra.Add(new TourVendorPromoModel()
                {
                    PromoValue = 0,
                    PromoPercent = 0
                });
            }

            var model = new SchedulerPriceModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,

                TourID = id ?? 0,
                ScheduleName = tourSchedule == null ? string.Empty : tourSchedule.Name,
                //BgnDate = tourSchedule == null ? DateTime.UtcNow.ToString(ConstantData.CommonTimeFormat) : tourSchedule.BeginDate.ToString(ConstantData.CommonTimeFormat),
                //EndDate = tourSchedule == null ? DateTime.UtcNow.ToString(ConstantData.CommonTimeFormat) : tourSchedule.EndDate.ToString(ConstantData.CommonTimeFormat),

                //Daterange = tourSchedule == null ? string.Empty : tourSchedule.DateRange,
                //IsMonday = tourSchedule == null ? false : tourSchedule.IsMonday,
                //IsTuesday = tourSchedule == null ? false : tourSchedule.IsTuesday,
                //IsWednesday = tourSchedule == null ? false : tourSchedule.IsWednesday,
                //IsThursday = tourSchedule == null ? false : tourSchedule.IsThursday,
                //IsFriday = tourSchedule == null ? false : tourSchedule.IsFriday,
                //IsSaturday = tourSchedule == null ? false : tourSchedule.IsSaturday,
                //IsSunday = tourSchedule == null ? false : tourSchedule.IsSunday,

                Daterange = tourSchedule ==
                        null ?
                        string.Format("{0} - {1}",
                        DateTime.Now.ToString(TimeHelper.DefaultDateFormat),
                        DateTime.Now.AddYears(2).ToString(TimeHelper.DefaultDateFormat))
                        :
                        tourSchedule.DateRange,
                IsMonday = tourSchedule == null ? true : tourSchedule.IsMonday,
                IsTuesday = tourSchedule == null ? true : tourSchedule.IsTuesday,
                IsWednesday = tourSchedule == null ? true : tourSchedule.IsWednesday,
                IsThursday = tourSchedule == null ? true : tourSchedule.IsThursday,
                IsFriday = tourSchedule == null ? true : tourSchedule.IsFriday,
                IsSaturday = tourSchedule == null ? true : tourSchedule.IsSaturday,
                IsSunday = tourSchedule == null ? true : tourSchedule.IsSunday,

                ExtraNames1 = tourExtrasSize > 0 ? tourExtras[0].Name : string.Empty,
                ExtraPrices1 = tourExtrasSize > 0 ? tourExtras[0].Price : 0,
                ExtraTimes1 = tourExtrasSize > 0 ? tourExtras[0].Time : 0,
                ExtraNames2 = tourExtrasSize > 1 ? tourExtras[1].Name : string.Empty,
                ExtraPrices2 = tourExtrasSize > 1 ? tourExtras[1].Price : 0,
                ExtraTimes2 = tourExtrasSize > 1 ? tourExtras[1].Time : 0,
                ExtraNames3 = tourExtrasSize > 2 ? tourExtras[2].Name : string.Empty,
                ExtraPrices3 = tourExtrasSize > 2 ? tourExtras[2].Price : 0,
                ExtraTimes3 = tourExtrasSize > 2 ? tourExtras[2].Time : 0,
                ExtraNames4 = tourExtrasSize > 3 ? tourExtras[3].Name : string.Empty,
                ExtraPrices4 = tourExtrasSize > 3 ? tourExtras[3].Price : 0,
                ExtraTimes4 = tourExtrasSize > 3 ? tourExtras[3].Time : 0,
                ExtraNames5 = tourExtrasSize > 4 ? tourExtras[4].Name : string.Empty,
                ExtraPrices5 = tourExtrasSize > 4 ? tourExtras[4].Price : 0,
                ExtraTimes5 = tourExtrasSize > 4 ? tourExtras[4].Time : 0,

                ExtraTimesType1 = tourExtrasSize > 0 ? tourExtras[0].TimeType : (int)TourTimeType.Days,
                ExtraTimesType2 = tourExtrasSize > 1 ? tourExtras[1].TimeType : (int)TourTimeType.Days,
                ExtraTimesType3 = tourExtrasSize > 2 ? tourExtras[2].TimeType : (int)TourTimeType.Days,
                ExtraTimesType4 = tourExtrasSize > 3 ? tourExtras[3].TimeType : (int)TourTimeType.Days,
                ExtraTimesType5 = tourExtrasSize > 4 ? tourExtras[4].TimeType : (int)TourTimeType.Days,

                TimesTypeOptions = TimeTypeTranslation.Translations.ToSelectList(x => x.TimeType, x => x.Code.ToString()),

                DiscountTourists = tourSchedule == null ? 0 : tourSchedule.DiscountTourists ?? 0,
                DiscountValue = tourSchedule == null ? 0 : tourSchedule.DiscountValue ?? 0,
                DiscountPercent = tourSchedule == null ? 0 : (float)(tourSchedule.DiscountPercent ?? 0),
                StartTime1 = tourSchedule == null ? "8:00am" : tourSchedule.StartTime1,
                StartTime2 = tourSchedule == null ? string.Empty : tourSchedule.StartTime2,
                StartTime3 = tourSchedule == null ? string.Empty : tourSchedule.StartTime3,
                StartTime4 = tourSchedule == null ? string.Empty : tourSchedule.StartTime4,
                StartTime5 = tourSchedule == null ? string.Empty : tourSchedule.StartTime5,
                StartTime6 = tourSchedule == null ? string.Empty : tourSchedule.StartTime6,

                NetPrice = tourSchedule == null ? 0 : (float)(tourSchedule.NetPrice ?? 0),
                SugRetailPrice = tourSchedule == null ? 0 : (float)(tourSchedule.SugRetailPrice ?? 0),
                CommisionPay = tourSchedule == null ? 10 : (float)(tourSchedule.CommisionPay ?? 0),
                TourPriceBreakdowns = tourPriceBreakdowns,
                TourPriceBreakdownsExtra = tourPriceBreakdownsExtra,
                TourVendorPromos = tourVendorPromos,
                TourVendorPromosExtra = tourVendorPromosExtra,
                IfShowTourPriceBreakdowns = (tour.BookingType==0),
                MinTouristNum = tour.MinTouristNum,
                MaxTouristNum = tour.MaxTouristNum,
                ID = tourSchedule == null ? 0 : tourSchedule.ID,
            };
            return View("SchedulerPrice", InitLayout(model));
        }

        public ActionResult SaveSchedulerPrice(SchedulerPriceModel model)
        {
            //Calculate comission pay based on NetPrice & SugRetailPrice
            model.NetPrice = (float)Math.Round(model.NetPrice);
            model.SugRetailPrice = (float)Math.Round(model.SugRetailPrice);
            model.CommisionPay = (float)Math.Round((model.SugRetailPrice / model.NetPrice - 1)*100);

            var tourschedule = _repositoryGuides.TourScheduleGetByTourID(model.TourID);
            if (tourschedule == null) { 
                tourschedule = new TourSchedule();
            }else {
                tourschedule.ModifyTime = DateTime.UtcNow;
            }
            tourschedule.TourID = model.TourID;
            //tourschedule.Name = model.ScheduleName;
            //Hardcoded ScheduleName
            tourschedule.Name = string.Format("{0}-ScheduleName", Guid.NewGuid().ToString().Substring(0, 8));
            tourschedule.DateRange = model.Daterange;
            DateTime start, end;
            TimeHelper.ConvertDateRangeStringToDateTime(model.Daterange, out start, out end);
            tourschedule.BeginDate = start;
            tourschedule.EndDate = end;
            tourschedule.IsMonday = model.IsMonday;
            tourschedule.IsTuesday = model.IsTuesday;
            tourschedule.IsWednesday = model.IsWednesday;
            tourschedule.IsThursday = model.IsThursday;
            tourschedule.IsFriday = model.IsFriday;
            tourschedule.IsSaturday = model.IsSaturday;
            tourschedule.IsSunday = model.IsSunday;
            tourschedule.DiscountTourists = model.DiscountTourists;
            tourschedule.DiscountValue = model.DiscountValue;
            tourschedule.DiscountPercent = model.DiscountPercent;
            tourschedule.StartTime1 = model.StartTime1;
            tourschedule.StartTime2 = model.StartTime2;
            tourschedule.StartTime3 = model.StartTime3;
            tourschedule.StartTime4 = model.StartTime4;
            tourschedule.StartTime5 = model.StartTime5;
            tourschedule.StartTime6 = model.StartTime6;
            tourschedule.NetPrice = model.NetPrice;
            tourschedule.SugRetailPrice = model.SugRetailPrice;
            tourschedule.CommisionPay = model.CommisionPay;

            _repositoryGuides.TourScheduleCreateOrUpdate(tourschedule);

            var extraNames = new List<string>() { model.ExtraNames1, model.ExtraNames2,
                        model.ExtraNames3, model.ExtraNames4, model.ExtraNames5 };
            var extraPrices = new List<int>() { model.ExtraPrices1, model.ExtraPrices2,
                        model.ExtraPrices3, model.ExtraPrices4, model.ExtraPrices5 };
            var extraTimes = new List<int>() { model.ExtraTimes1, model.ExtraTimes2,
                        model.ExtraTimes3, model.ExtraTimes4, model.ExtraTimes5 };
            var extraTimesTypes = new List<int>() { model.ExtraTimesType1, model.ExtraTimesType2,
                        model.ExtraTimesType3, model.ExtraTimesType4, model.ExtraTimesType5 };
            for (byte i = 0; i < extraNames.Count; i++)
            {
                CheckAndSaveTourExtra(model.TourID, (byte)(i + 1), extraNames[i], extraPrices[i], extraTimes[i], (byte)extraTimesTypes[i]);
            }
            #region process allTourPriceBreakdown
            List<TourPriceBreakdownModel> allTourPriceBreakdowns = null;
            if (model.TourPriceBreakdowns == null && model.TourPriceBreakdownsExtra == null)
            {
                allTourPriceBreakdowns = new List<TourPriceBreakdownModel>();
            }
            else if (model.TourPriceBreakdowns == null)
            {
                allTourPriceBreakdowns = model.TourPriceBreakdownsExtra;
            }
            else if (model.TourPriceBreakdownsExtra == null)
            {
                allTourPriceBreakdowns = model.TourPriceBreakdowns;
            }
            else
            {
                allTourPriceBreakdowns = model.TourPriceBreakdowns.Concat(model.TourPriceBreakdownsExtra).ToList();
            }

            allTourPriceBreakdowns = allTourPriceBreakdowns.OrderBy(x => x.EndPoint1).ToList();//Sort User's input by lower bound EndPoint1
            int tourPriceBreakdownCount = 0;
            for (int i = 0; i < allTourPriceBreakdowns.Count; i++)
            {
                if (allTourPriceBreakdowns[i].DiscountValue == 0 && allTourPriceBreakdowns[i].DiscountPercent==0)
                {
                    //CheckAndDeleteTourPriceBreakdown
                    continue;
                }
                CheckAndSaveTourPriceBreakdowns(tourId:model.TourID, sortNo:(byte)(tourPriceBreakdownCount + 1), theTourPriceBreakdown:allTourPriceBreakdowns[i]);
                tourPriceBreakdownCount++;
            }
            //Delete old unused ones with SortNo > (byte)(tourPriceBreakdownCount + 1)
            DeleteOldTourPriceBreakdowns(tourId: model.TourID, sortNo: (byte)(tourPriceBreakdownCount + 1));
            #endregion

            #region process TourVendorPromo
            List<TourVendorPromoModel> allTourVendorPromos = null;
            if (model.TourVendorPromos == null && model.TourVendorPromosExtra == null) {
                allTourVendorPromos = new List<TourVendorPromoModel>();
            } else if (model.TourVendorPromos == null) {
                allTourVendorPromos = model.TourVendorPromosExtra;
            } else if (model.TourVendorPromosExtra == null) {
                allTourVendorPromos = model.TourVendorPromos;
            } else {
                allTourVendorPromos = model.TourVendorPromos.Concat(model.TourVendorPromosExtra).ToList();
            }

            allTourVendorPromos = allTourVendorPromos
                //.Where(x=>x.DateRange!=(string.Format("{0} - {0}", DateTime.Now.Date.ToString(TimeHelper.DefaultDateFormat))))
                .OrderBy(x => x.DateRange).ToList();//Sort User's input by lower bound EndPoint1
            int tourVendorPromoCount = 0;
            for(int i = 0; i < allTourVendorPromos.Count; i++)
            {
                if (allTourVendorPromos[i].PromoValue == 0 && allTourVendorPromos[i].PromoPercent.Equals(0))
                {
                    //CheckAndDeleteTourVendorPromo
                    continue;
                }

                CheckAndSaveTourVendorPromos(tourId:model.TourID, sortNo:(byte)(tourVendorPromoCount + 1), theTourVendorPromo:allTourVendorPromos[i]);
                tourVendorPromoCount++;
            }
            //Delete old unused ones with SortNo > (byte)(tourVendorPromoCount + 1)
            DeleteOldTourVendorPromos(tourId:model.TourID, sortNo:(byte)(tourVendorPromoCount + 1));
            #endregion
            return RedirectToAction("Pictures", new { id = model.TourID });
        }

        public ActionResult ShowPictures(Nullable<int> id)
        {
            List<TourPicture> tourPictures = null;
            if (id != null)
            {
                tourPictures = _repositoryGuides.TourPictureGetAllByTourID(id.Value).ToList();
            }
            var model = new PicturesModel
            {
                IsMember = false,
                IsSignedIn = false,
                SelectedPage = LayoutSelectedPage.Account,
                TourID = id ?? 0,
                Pictures = tourPictures.Select(x => new SinglePicture()
                {
                    Url = Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.RelativePath),
                    ID = x.ID,
                    TourID = x.TourID,
                }).ToList()
            };
            return View("Pictures", InitLayout(model));
        }

        public ActionResult SavePictures(PicturesModel model)
        {
            _repositoryGuides.TourUpdateStatus(model.TourID, (byte)TourStatus.Complete);
            return RedirectToAction("Publish", new { id = model.TourID });
        }


        public ActionResult ShowMiscellaneous(Nullable<int> id)
        {
            Tour tour = null;
            if (id != null)
            {
                tour = _repositoryGuides.TourGetByID(id.Value);
            }

            if (tour==null)
            {
                Log.Error("Tour #" + id.Value + " is not found in DB table.");
                return RedirectToAction("InternalError", "Static", null);
            }
            var guide = _repositoryUsers.GetByIdOrNull(tour.UserID);
            var result = VerifyGuiderContactInfo(guide, true, new DoneRedirect());

            var model = new MiscellaneousModel
            {
                TourID = tour.ID,
                UserID = _security.GetCurrentUserId(),
                GuideID = tour.UserID,
                IsEmailAndPhoneVerified = (result == null) ? true : false,
                IfActive = true
            };
            return View("Publish", InitLayout(model));
        }

        //public ActionResult SaveMiscellaneous(MiscellaneousModel model)
        //{
        //    _repositoryGuides.TourUpdateStatus(model.TourID, (byte)TourStatus.Complete);
        //    return RedirectToAction("TourProducts");
        //}
        public void CheckAndSaveTourInclusion(int tourId, byte sortNo, string name)
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

        public void CheckAndSaveTourExclusion(int tourId, byte sortNo, string name)
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

        public void CheckAndSaveTourExtra(int tourId, byte sortNo, string name, int price, int time, byte timetype)
        {
            //if (!string.IsNullOrEmpty(name))
            //{
            var tourExtra = _repositoryGuides.TourExtraGetByTourIDSortNo(tourId, sortNo);
            if (tourExtra == null)
            {
                tourExtra = new TourExtra()
                {
                    Name = name ?? string.Empty,
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

        public void CheckAndSaveTourPriceBreakdowns(int tourId, byte sortNo, TourPriceBreakdownModel theTourPriceBreakdown)
        {
            if (theTourPriceBreakdown!=null)
            {
                var tourPriceBreakdown = _repositoryGuides.TourPriceBreakdownGetByTourIDSortNo(tourId, sortNo);
                if (tourPriceBreakdown == null)
                {
                    tourPriceBreakdown = new TourPriceBreakdown();
                    tourPriceBreakdown.TourID = tourId;
                    tourPriceBreakdown.EndPoint1 = theTourPriceBreakdown.EndPoint1;
                    tourPriceBreakdown.EndPoint2 = theTourPriceBreakdown.EndPoint2;
                    tourPriceBreakdown.DiscountValue = theTourPriceBreakdown.DiscountValue;
                    tourPriceBreakdown.DiscountPercent = theTourPriceBreakdown.DiscountPercent;
                    tourPriceBreakdown.SortNo = sortNo;
                    //tourPriceBreakdown.BeginDate = DateTime.MinValue;
                    //tourPriceBreakdown.EndDate = DateTime.MaxValue;
                    //tourPriceBreakdown.DateRange = string.Format("{0} - {1}",
                    //    tourPriceBreakdown.BeginDate.ToString(TimeHelper.DefaultDateFormat),
                    //    tourPriceBreakdown.EndDate.ToString(TimeHelper.DefaultDateFormat)
                    //);
                    tourPriceBreakdown.EnterTime = DateTime.UtcNow;
                    tourPriceBreakdown.ModifyTime = DateTime.UtcNow;
                }
                else
                {
                    tourPriceBreakdown.EndPoint1 = theTourPriceBreakdown.EndPoint1;
                    tourPriceBreakdown.EndPoint2 = theTourPriceBreakdown.EndPoint2;
                    tourPriceBreakdown.DiscountValue = theTourPriceBreakdown.DiscountValue;
                    tourPriceBreakdown.DiscountPercent = theTourPriceBreakdown.DiscountPercent;
                    //tourPriceBreakdown.BeginDate = DateTime.MinValue;
                    //tourPriceBreakdown.EndDate = DateTime.MaxValue;
                    //tourPriceBreakdown.DateRange = string.Format("{0} - {1}",
                    //    tourPriceBreakdown.BeginDate.ToString(TimeHelper.DefaultDateFormat),
                    //    tourPriceBreakdown.EndDate.ToString(TimeHelper.DefaultDateFormat)
                    //);
                    tourPriceBreakdown.ModifyTime = DateTime.UtcNow;
                }
                _repositoryGuides.TourPriceBreakdownCreateOrUpdate(tourPriceBreakdown);
            }
        }


        public void CheckAndSaveTourVendorPromos(int tourId, byte sortNo, TourVendorPromoModel theTourVendorPromo)
        {
            if (theTourVendorPromo != null)
            {
                var tourVendorPromo = _repositoryGuides.TourVendorPromoGetByTourIDSortNo(tourId, sortNo);
                if (tourVendorPromo == null) {
                    tourVendorPromo = new TourVendorPromo();
                    tourVendorPromo.EnterTime = DateTime.UtcNow;
                }
                var theDateRange = theTourVendorPromo.DateRange;
                DateTime start, end;
                TimeHelper.ConvertDateRangeStringToDateTime(theDateRange, out start, out end);

                tourVendorPromo.DateRange = theDateRange;
                if (start.Equals(end))
                {
                    //For instance, Change 2017-01-23 00:00:00.000 - 2017-01-23 00:00:00.000
                    //To 2017-01-23 00:00:00.000 - 2017-01-23 23:59:59.000
                    end = end.AddDays(1).AddSeconds(-1);
                }
                else
                {
                    //For instance, Change 2017-01-23 00:00:00.000 - 2017-01-25 00:00:00.000
                    //To 2017-01-23 00:00:00.000 - 2017-01-24 23:59:59.000
                    end = end.AddSeconds(-1);
                }
                //theDateRange = string.Format("{0} - {1}",
                //    start.ToString(TimeHelper.DefaultDateFormat),
                //    end.ToString(TimeHelper.DefaultDateFormat)
                //);

                tourVendorPromo.BeginDate = start;
                tourVendorPromo.EndDate = end;
                
                tourVendorPromo.PromoValue = theTourVendorPromo.PromoValue;
                tourVendorPromo.PromoPercent = theTourVendorPromo.PromoPercent;
                tourVendorPromo.ModifyTime = DateTime.UtcNow;
                tourVendorPromo.SortNo = sortNo;
                tourVendorPromo.TourID = tourId;

                _repositoryGuides.TourVendorPromoCreateOrUpdate(tourVendorPromo);
            }
        }

        #region Validition methods

        public void DeleteOldTourInclusions(int tourId, byte sortNo)
        {
            _repositoryGuides.TourInclusionDeleteOld(tourId, sortNo);
        }
        public void DeleteOldTourExclusions(int tourId, byte sortNo)
        {
            _repositoryGuides.TourExclusionDeleteOld(tourId, sortNo);
        }

        public void DeleteOldTourPriceBreakdowns(int tourId, byte sortNo)
        {
            _repositoryGuides.TourPriceBreakdownDeleteOld(tourId, sortNo);
        }
        public void DeleteOldTourVendorPromos(int tourId, byte sortNo)
        {
            _repositoryGuides.TourVendorPromoDeleteOld(tourId, sortNo);
        }
        //public void ValidTourType(TourTypeModel model)
        //{
        //    int typeCount = 0;
        //    bool[] types = new bool[] {
        //        model.IsHistorical,model.IsAdventure,model.IsLeisureSports,
        //        model.IsCultureArts,model.IsNatureRural,model.IsFestivalEvents,
        //        model.IsNightlifeParty,model.IsFoodDrink,model.IsShoppingMarket,
        //        model.IsTransportation,model.IsBusinessInterpretation,model.IsPhotography
        //    };
        //    foreach (var type in types)
        //    {
        //        if (type)
        //        {
        //            typeCount++;
        //        }
        //    }
        //    if (model.LanguageIDs == null || model.LanguageIDs.Length <= 0)
        //    {
        //        ModelState.AddModelError("LanguageValidMsg", ValidationStrings.TourLanguageSelection);
        //    }
        //    if (typeCount <= 0 || typeCount >= 6)
        //    {
        //        ModelState.AddModelError("TypeValidMsg", ValidationStrings.TourTypeSelection);
        //    }
        //    return;
        //}

        //public void ValidTourOverview(TourOverviewModel model)
        //{
        //    if (string.IsNullOrEmpty(model.TourCityHidden) && string.IsNullOrEmpty(model.TourCity))
        //    {
        //        ModelState.AddModelError("TourCity", string.Format(ValidationStrings.Required, "TourCity"));
        //    }
        //    int i = 0;
        //    foreach (var v in model.TourInclusionsExtra)
        //    {
        //        if (v?.Name?.Length < 1 || v?.Name?.Length > 300)
        //        {
        //            ModelState.AddModelError("TourInclusionExtraValidationMessage" + i, GuideStrings.TourInclusionExtraValidationMessage);
        //        }
        //        i++;
        //    }

        //    i = 0;
        //    foreach (var v in model.TourExclusionsExtra)
        //    {
        //        if (v?.Name?.Length < 1 || v?.Name?.Length > 300)
        //        {
        //            ModelState.AddModelError("TourExclusionExtraValidationMessage" + i, GuideStrings.TourExclusionExtraValidationMessage);
        //        }
        //        i++;
        //    }
        //    return;
        //}

        //public void ValidBookingDetails(BookingDetailsModel model)
        //{
        //    if (model.MinTouristNum <= 0 || model.MinTouristNum > model.MaxTouristNum)
        //    {
        //        ModelState.AddModelError("MinTouristNum", ValidationStrings.MinimumTouristNo);
        //    }
        //    if (model.MaxTouristNum <= 0 || model.MinTouristNum > model.MaxTouristNum)
        //    {
        //        ModelState.AddModelError("MaxTouristNum", ValidationStrings.MaximumTouristNo);
        //    }
        //    if (model.MinHourAdvance <= 0)
        //    {
        //        ModelState.AddModelError("MinHourAdvance", ValidationStrings.MinHourAdvance);
        //    }
        //    return;
        //}

        //public void ValidSchedulerPrice(SchedulerPriceModel model)
        //{
        //    if ((model.ExtraPrices1 != 0 || model.ExtraTimes1 != 0) && string.IsNullOrEmpty(model.ExtraNames1))
        //    {
        //        ModelState.AddModelError("ExtraNames1", string.Format(ValidationStrings.Required, "ExtraNames1"));
        //    }
        //    if ((model.ExtraPrices2 != 0 || model.ExtraTimes2 != 0) && string.IsNullOrEmpty(model.ExtraNames2))
        //    {
        //        ModelState.AddModelError("ExtraNames2", string.Format(ValidationStrings.Required, "ExtraNames2"));
        //    }
        //    if ((model.ExtraPrices3 != 0 || model.ExtraTimes3 != 0) && string.IsNullOrEmpty(model.ExtraNames3))
        //    {
        //        ModelState.AddModelError("ExtraNames3", string.Format(ValidationStrings.Required, "ExtraNames3"));
        //    }
        //    if ((model.ExtraPrices4 != 0 || model.ExtraTimes4 != 0) && string.IsNullOrEmpty(model.ExtraNames4))
        //    {
        //        ModelState.AddModelError("ExtraNames4", string.Format(ValidationStrings.Required, "ExtraNames4"));
        //    }
        //    if ((model.ExtraPrices5 != 0 || model.ExtraTimes5 != 0) && string.IsNullOrEmpty(model.ExtraNames5))
        //    {
        //        ModelState.AddModelError("ExtraNames5", string.Format(ValidationStrings.Required, "ExtraNames5"));
        //    }
        //    return;
        //}

        //public void ValidPictures(PicturesModel model)
        //{
        //    var tourPictures = _repositoryGuides.TourPictureGetAllByTourID(model.TourID).ToList();
        //    if (tourPictures.Count < 3)
        //        ModelState.AddModelError("ValidateInfo", "Please upload at least 3 pictures for your tour");
        //    return;
        //}

        public void ValidMiscellaneous(MiscellaneousModel model)
        {
            return;
        }
        #endregion

        public ActionResult VerifyGuiderContactInfo(User user, bool verifyPhone, DoneRedirect redirect)
        {
            if ((user.OpenSite == 0 && !user.IsConfirmed) || (user.OpenSite > 0 && !user.IsEmailVerified4OpenID))
            {
                return RedirectToAction("ConfirmEmail", "Account", new { DoneActionName = redirect.DoneActionName, DoneControllerName = redirect.DoneControllerName });
            }

            if (!verifyPhone)
                return null;

            if (!user.IsPhoneConfirmed)
            {
                return RedirectToAction("ConfirmPhone", "Account", new  { DoneActionName = redirect.DoneActionName, DoneControllerName = redirect.DoneControllerName });
            }
            return null;
        }

        //public ActionResult TourSetStatus(int id, byte status, DoneRedirect redirect)
        //{
        //    try
        //    {
        //        if (status == (byte)TourStatus.Active)
        //        {
        //            var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
        //            var result = VerifyGuiderContactInfo(user, true, redirect);
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
        //        _repositoryGuides.TourUpdateStatus(id, status); //Actual Status Update
        //        return RedirectToAction(redirect.NextActionName);
        //    }
        //    catch (Exception excp)
        //    {
        //        _logger.LogError(excp);
        //        return RedirectToAction("InternalError", "Static", null);
        //    }
        //}

        public List<T> ConcatLists<T>(List<T> list1, List<T> list2)
        {
            List<T> consolidatedList = null;
            if (list1 == null && list2 == null)
            {
                consolidatedList = new List<T>();
            }
            else if (list1 == null)
            {
                consolidatedList = list2;
            }
            else if (list2 == null)
            {
                consolidatedList = list1;
            }
            else
            {
                consolidatedList = list1.Concat(list2).ToList();
            }

            return consolidatedList;
        }
    }
}