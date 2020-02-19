using MVCSite.Biz;
using MVCSite.Biz.Interfaces;
using MVCSite.Common;
using MVCSite.DAC.Common;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Extensions;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Repositories;
using MVCSite.Web.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.UI;
using MVCSite.ViewResource;
using NLog.Internal;
using MVCSite.Common.NameHelper;
using Org.Mentalis.Proxy.Http;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Web.Configuration;
using System.Windows.Forms.VisualStyles;
using DevTrends.MvcDonutCaching;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using MVCSite.Web.Services;

namespace MVCSite.Web.Controllers
{
    public class TouristController : LayoutBase
    {
        private readonly IPublicCommands _commands;
        protected readonly IRepositoryGuides _repositoryGuides;
        protected readonly IRepositoryTourists _repositoryTourists;
        protected readonly IRepositoryCities _repositoryCities;
        protected readonly IRepositoryTypes _repositoryTypes;
        protected readonly IRepositoryPromos _repositoryPromos;

      
        public const string _StripePublicKey = "pk_live_bH8kJazCDEUyWkBWSmX64K7E";
        public const string _StripeSecretKey = "sk_live_m03eVWFIp3auFEeOdMXOhwSk";
        public const string _StripePublicKeyTEST = "pk_test_qgo97HjJgIKh4TqgUwZA6xnX";
        public const string _StripeSecretKeyTEST = "sk_test_A82k7glNBuQFwGIX058JY5sw";
   
        
       


        public TouristController(ISecurity security, IWebApplicationContext webContext,
            IRepositoryUsers repositoryUsers,
            IRepositoryGuides repositoryGuides,
            IRepositoryTourists repositoryTourists,
            IRepositoryCities repositoryCities,
            IRepositoryTypes repositoryTypes,
            IRepositoryPromos repositoryPromos,
            IPublicCommands commands,
            ISiteConfiguration configuration, ILogger logger)
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
            _commands = commands;
            _repositoryGuides = repositoryGuides;
            _repositoryTourists = repositoryTourists;
            _repositoryCities = repositoryCities;
            _repositoryTypes = repositoryTypes;
            _repositoryPromos = repositoryPromos;
        }
        //[OutputCache(Duration=60, Location = OutputCacheLocation.Any)]
        public ActionResult Index()
        {
            try
            {

                List<SelectListItem> options = _repositoryCities.GetAllCitiesInUseCached()
                    .ToSelectList(x => x.Name, x => x.CityId.ToString());
                var featuredTours = _repositoryGuides.GetAllFeaturedToursCached();

                var model = new IndexModel()
                {
                    SelectCityID = 0,
                    CityOptions = options,
                    SelectCity = "Vancouver,BC,Canada",
                    SelectCityHidden = "Vancouver,BC,Canada",
                    SelectDate = DateTime.UtcNow.ToString(TimeHelper.DefaultDateFormat),
                    IsIndex = true,
                    IsMobile = Request.Browser.IsMobileDevice,
                    FeaturedTours = featuredTours
                };
                return View("Index", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
            finally
            {
                TempData.Clear();
            }
        }
        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.SelectCityHidden) || model.SelectCityID == 0)
                    ModelState.AddModelError("SelectCity", "Please select a destination");
                if (string.IsNullOrEmpty(model.SelectDate))
                    ModelState.AddModelError("SelectDate", "Please select a date");
                if (ModelState.IsValid)
                {
                    City city = null;
                    if (model.SelectCityID > 0)
                    {
                        city = _repositoryCities.GetCityByIdInDB(model.SelectCityID);
                    }
                    else if (!string.IsNullOrEmpty(model.SelectCityHidden))
                    {
                        city = _repositoryCities.GetCityByUniqueNameOrNullInDB(model.SelectCityHidden);
                    }
                    var cityId = city == null ? 0 : city.CityId;

                    TempData["ToursModel"] = new ToursModel
                    {
                        SelectCityID = model.SelectCityID,
                        SelectCity = model.SelectCity,
                        SelectDate = model.SelectDate,
                        CategoryID = 1,
                        LanguageID = 0,
                        SearchMode = SearchMode.ByCity,
                    };
                    return RedirectToAction("Tours", new
                    {
                        //countryname = city?.UniqueCityName.Split(',')[1] ?? model.SelectCityHidden.Split(',')[1],
                        //cname = city?.UniqueCityName.Split(',')[0] ?? model.SelectCityHidden.Split(',')[0]
                        cname = NameHelper.SpaceToDash(NameHelper.GetCityName(city?.UniqueCityName ?? model.SelectCityHidden))
                    });
                    //return RedirectToAction("Tours", new {
                    //    id = cityId,
                    //    cname = city?.Name,
                    //    date = model.SelectDate,
                    //    cat = 1,
                    //    lan = 0,
                    //});
                }
                model.CityOptions = _repositoryCities.GetAllCitiesInUseCached().ToSelectList(x => x.Name, x => x.CityId.ToString());
                return View("Index", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        public ActionResult Tours(string cname, int id = 0, int cat = 1, int dur = 0, int sortBy = 0)//, SearchMode searchMode = SearchMode.ByCity 
        {
            try
            {
                SearchMode searchMode;
                var criteria = new SearchCriteria();
                var cityName = string.Empty;
                City targetCity;

                cname = NameHelper.SpaceToDash(cname);//Dashed fomart to compare with dasded counterpart in DB
                if (true)
                {
                    if (id > 0)
                    {
                        targetCity = _repositoryCities.GetCityByIdInDB(id);
                    }
                    else if (!string.IsNullOrEmpty(cname))
                    {
                        //targetCity = _repositoryCities.GetCityByUniqueNameOrNullInDB(cname);
                        targetCity = _repositoryCities.GetCityByUniqueCityNameOrNullInDBViaDashedNames(cname);
                    }
                    else
                    {
                        return RedirectToAction("InternalError", "Static", null);
                    }

                    if (targetCity == null)
                    {
                        id = 0;
                        //cityName = cname;
                        cityName = NameHelper.DashToSpace(cname);//May not good for city like Shangli-La
                    }
                    else
                    {
                        id = targetCity.CityId;
                        cityName = targetCity.UniqueCityName;
                    }
                    criteria.Cityid = id;
                    searchMode = SearchMode.ByCity;
                }

                ToursModel toursModel = TempData["ToursModel"] as ToursModel;
                string date = toursModel?.SelectDate ?? DateTime.Now.Date.ToString("MM/dd/yyyy");
                //id = toursModel?.SelectCityID ?? id;
                if (id == 0 && toursModel?.SelectCityID > 0)
                {
                    id = toursModel.SelectCityID;
                }
                cat = toursModel?.CategoryID ?? cat;
                dur = toursModel?.Duration ?? dur;
                searchMode = toursModel?.SearchMode ?? searchMode;

                TempData.Keep("ToursModel");

                switch ((TourCategory)cat)
                {
                    default:
                    case TourCategory.All:
                        criteria.IsAllCategory = true;
                        break;
                    case TourCategory.Adventure:
                        criteria.IsAdventure = true;
                        break;
                    case TourCategory.CultureArts:
                        criteria.IsCultureArts = true;
                        break;
                    case TourCategory.FestivalEvents:
                        criteria.IsFestivalEvents = true;
                        break;
                    case TourCategory.FoodDrink:
                        criteria.IsFoodDrink = true;
                        break;
                    case TourCategory.Historical:
                        criteria.IsHistorical = true;
                        break;
                    case TourCategory.LeisureSports:
                        criteria.IsLeisureSports = true;
                        break;
                    case TourCategory.NatureRural:
                        criteria.IsNatureRural = true;
                        break;
                    case TourCategory.NightlifeParty:
                        criteria.IsNightlifeParty = true;
                        break;
                    case TourCategory.ShoppingMarket:
                        criteria.IsShoppingMarket = true;
                        break;
                    case TourCategory.Transportation:
                        criteria.IsTransportation = true;
                        break;
                    case TourCategory.BusinessInterpretation:
                        criteria.IsBusinessInterpretation = true;
                        break;
                    case TourCategory.Photography:
                        criteria.IsPhotography = true;
                        break;
                }


                criteria.Duration = dur;
                var model = new ToursModel()
                {
                    SelectCityID = id,
                    SelectCity = cityName,
                    SelectDate = date,
                    CategoryID = cat,
                    CategoryOptions = TourCategoryTranslation.Translations.ToSelectList(x => x.Category, x => x.Code.ToString()),
                    Duration = dur,
                    SortByPrice=sortBy,
                    LanguageOptions = LanguageTranslation.Translations.ToSelectList(x => x.Language, x => x.Code.ToString()),
                    SearchMode = searchMode,
                };

                //First, Process page elements
                var landingFolder = System.Configuration.ConfigurationManager.AppSettings["landing"];
                switch (searchMode)
                {
                    case SearchMode.ByCity:
                        if (!string.IsNullOrEmpty(cityName) && cityName.Contains(","))
                            model.BannerTitle = cityName.Split(',')[0];
                        else
                            model.BannerTitle = cityName;

                        List<City> destinationList = _repositoryCities
                            .GetCityListByIdList(targetCity?.DestinationList.SplitToIntArray("|"))
                            ?.Where(x => x.CityId != targetCity?.CityId).ToList();

                        Country theCountry = _repositoryCities.GetCountryById(targetCity?.CountryID);
                        List<City> theCountryDestinationList = _repositoryCities
                            .GetCityListByIdList(theCountry?.DestinationList.SplitToIntArray("|"))
                            ?.Where(x => x.CityId != targetCity?.CityId).ToList();

                        if (targetCity != null && !string.IsNullOrEmpty(targetCity.Intro))
                        {
                            var nameOfCity = targetCity.UniqueCityName.Split(',')[0];
                            nameOfCity = NameHelper.GenDriveName(nameOfCity);
                            if (Directory.Exists(Server.MapPath(string.Format(@"~/images/{0}/{1}", landingFolder, nameOfCity)))
                                && System.IO.File.Exists(Server.MapPath(string.Format(@"~/images/{0}/{1}/{1}.jpg", landingFolder, nameOfCity))))
                            {
                                model.BannerImageName = nameOfCity;
                            }
                            else
                            {
                                model.BannerImageName = "Generic";
                            }
                            model.Intro = targetCity.Intro;
                            model.Tip1 = targetCity.Tip1;
                            model.Tip2 = targetCity.Tip2;
                            model.Tip3 = targetCity.Tip3;
                            model.DestinationList = destinationList;
                            //model.BlogList = targetCity.BlogList;
                        }
                        else
                        {
                            var nameOfCountry = theCountry?.description;
                            nameOfCountry = NameHelper.GenDriveName(nameOfCountry);
                            if (Directory.Exists(Server.MapPath(string.Format(@"~/images/{0}/{1}", landingFolder, nameOfCountry)))
                                && System.IO.File.Exists(Server.MapPath(string.Format(@"~/images/{0}/{1}/{1}.jpg", landingFolder, nameOfCountry))))
                            {
                                model.BannerImageName = nameOfCountry;
                                //model.Intro = theCountry?.Intro;
                            }
                            else
                            {
                                model.BannerImageName = "Generic";
                                model.Intro = theCountry?.Intro;//Use Defult Generic Intro Stored For Country Intro
                            }

                            model.Tip1 = theCountry?.Tip1;
                            model.Tip2 = theCountry?.Tip2;
                            model.Tip3 = theCountry?.Tip3;
                            model.DestinationList = theCountryDestinationList;
                            //model.BlogList = theCountry?.BlogList;
                        }
                        break;
                    case SearchMode.ByCountry:
                        //model.BannerTitle = theCountry?.description;
                        break;
                    case SearchMode.ByCategory:
                        //model.BannerTitle = SelectedCity;
                        break;
                }

                //Second, Process each tour
                if (id == 0)//If id remains 0
                {
                    return RedirectToAction("NotFound", "Static", null);
                    //model.Tours = new List<Tour>().ToPageable(1, _ListViewPageSize, 0);
                    //return View("Tours", InitLayout(model));
                }
                IPageable<Tour> tours = _repositoryGuides.TourGetAllBySearchModeByPage(criteria, 1, _ListViewPageSize, (byte)searchMode);

                if (tours.Any())
                {
                    var tourIds = tours.Select(x => x.ID.ToString()).ToArray().Aggregate((prev, next) => prev + "," + next);
                    List<Tour> tourList;
                    List<TourPicture> tourPictureList;
                    List<TourSchedule> tourScheduleList;
                    List<TourVendorPromo> tourVendorPromoList;
                    List<User> userList;
                    RepositoryBigQueries.tourGetAllSimpleInfoByIDs(tourIds,
                        out tourList,
                        out tourPictureList,
                        out tourScheduleList,
                        out tourVendorPromoList,
                        out userList);
                    tours.ForEach(tour =>
                    {
                        var picture = tourPictureList.Where(x => x.TourID == tour.ID).OrderBy(x => x.ID).FirstOrDefault();
                        tour.ImagePath = picture != null ? picture.RelativePath : string.Empty;
                        var schedulerPrice = tourScheduleList.Where(x => x.TourID == tour.ID).FirstOrDefault();
                        tour.SugRetailPrice = schedulerPrice != null ? schedulerPrice.SugRetailPrice ?? 0 : 0;
                        var tourVendorPromoSubset = tourVendorPromoList.Where(x => x.TourID == tour.ID);
                        tour.NowPrice = CalculateVendorPromotedPrice(tour.SugRetailPrice, tourVendorPromoSubset);
                        //tour.NowPrice = schedulerPrice != null ? schedulerPrice.SugRetailPrice ?? 0 : 0;

                        var guider = userList.Where(x => x.ID == tour.UserID).FirstOrDefault();
                        var abbreviatedLastName = string.IsNullOrWhiteSpace(guider.LastName) ? "" : guider.LastName.Substring(0, 1) + ".";
                        var firstNameAbbreviatedLastName = string.Format("{0} {1}", guider.FirstName, abbreviatedLastName);

                        //tour.UserName = tourUser == null ? string.Empty : string.Format("{0} {1}", guider.FirstName, guider.LastName);
                        tour.UserName = firstNameAbbreviatedLastName;
                    });
                  
                }


             
                switch (sortBy)
                {
                    case 1:
                        model.Tours = tours.OrderBy(x => x.NowPrice).ToList();///Low to High
                        break;
                    case 2:
                        model.Tours = tours.OrderByDescending(x => x.NowPrice).ToList();///High to Low
                        break;

                    default:

                        model.Tours =  tours.GroupBy(grp => grp.TourVendorPromoes.Count > 0)
                              .SelectMany(g => g.OrderBy(grp => grp.NowPrice)).ToList();
                        break;
                }

                var durationList = new List<SelectType>();
                durationList.Add(new SelectType() { Value = "0", Text = "Select Duration" });
                durationList.Add(new SelectType() { Value = "1", Text = "Half day" });
                durationList.Add(new SelectType() { Value = "2", Text = "One day" });
                durationList.Add(new SelectType() { Value = "3", Text = "Multi days" });
                ViewBag.durationList = durationList;

                var sortByPriceList = new List<SelectType>();
                sortByPriceList.Add(new SelectType() { Value = "0", Text = "Sort By" });
                sortByPriceList.Add(new SelectType() { Value = "1", Text = "Price (low to high)" });
                sortByPriceList.Add(new SelectType() { Value = "2", Text = "Price (high to low)" });
                ViewBag.sortByPriceList = sortByPriceList;

                return View("Tours", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        [HttpPost]
        //[Route("{cname}/{cat?}", Order = 1)]
        //[Route("Tourist/Tours/{countryname?}/{cname?}/{cat?}", Order = 1)]
        public ActionResult Tours(ToursModel model)
        {
            try
            {
                TempData["ToursModel"] = model;
                TempData.Keep("ToursModel");
                return RedirectToAction("Tours", new
                {
                    cname = model.SelectCity?.Split(',')[0],
                    id = model.SelectCityID,
                    cat = model.CategoryID,
                    dur = model.Duration,
                    sortby = model.SortByPrice,
                });
                //return RedirectToAction("Tours", new
                //{
                //    id = model.SelectCityID,
                //    cname = model.SelectCity,
                //    date = model.SelectDate,
                //    cat = model.CategoryID,
                //    lan = model.LanguageID
                //});
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        public ActionResult Booking(int id, int tourId, int userId, bool IsGuestBooking)
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
                //var user = _repositoryUsers.GetByIdOrNull(tourObject.UserID);
                //var picture = tourPictureList.Where(x => x.TourID == tourObject.ID).OrderBy(x => x.ID).FirstOrDefault();

                //Revert back to TotalPay without applying any Promo Code through TotalPay = SubTotal + ServiceFee
                UserTourBooking currBooking = _repositoryGuides.UserTourBookingGetByID(id);
                currBooking.PromoPrice = 0;
                currBooking.TotalPay = currBooking.SubTotal + currBooking.ServiceFee;
                var modifiedBooking = _repositoryGuides.UserTourBookingCreateOrUpdate(currBooking);
                if (modifiedBooking == null)
                {
                    RedirectToAction("InternalError", "Static", null);
                }

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
                var userWish = _repositoryTourists.UserWishGetByUserIdTourId(userId, tourId);
                var IsDataSaved = true;
                if (userWish == null)
                {
                    IsDataSaved = false;
                }
                var tourCity = _repositoryCities.GetCityByIdInDB(tourObject.TourCityID);
                var totalTravelers = currBooking.Travellers;
                var traveler = _repositoryUsers.GetByIdOrNull(userId);
                var guider = _repositoryUsers.GetByIdOrNull(currBooking.TourUserID);
                var tour = _repositoryGuides.TourGetByID(tourId);

                var requestedDateTime = DateTime.ParseExact(currBooking.Calendar, TimeHelper.DefaultDateFormat, CultureInfo.InvariantCulture);
                var model = new BookingModel
                {
                    TourName = currBooking.TourName,
                    ImageUrl = currBooking.TourImgUrl,
                    Date = currBooking.Calendar,
                    Time = currBooking.Time,
                    Location = currBooking.Location,
                    MeetupLocation = tour.MeetupLocation,
                    TourLocationSimple = tourCity != null ? tourCity.Name : string.Empty,
                    City = currBooking.Location,

                    TravelerName = string.Format("{0} {1}", traveler.FirstName, traveler.LastName),
                    TravelerEmail = traveler.Email,
                    TravelerPhoneAreaCode = traveler.PhoneAreaCode,
                    TravelerMobile = traveler.Mobile,

                    GuideName = currBooking.GuiderName,
                    GuideID = guider.ID,
                    GuideAvatarPath = guider.AvatarPath,
                    GuideEmail = guider.Email,
                    GuidePhoneAreaCode = guider.PhoneAreaCode,
                    GuideMobile = guider.Mobile,
                    BookingType = tourObject.BookingType,
                    TotalTravelers = totalTravelers,
                    TotalCost = (float)tourScheduleObject?.SugRetailPrice,
                    TourCost = (float)tourScheduleObject.SugRetailPrice,
                    VendorPromoTourCost = (float)CalculateVendorPromotedPriceOnDate((float)tourScheduleObject.SugRetailPrice, tourVendorPromoList, requestedDateTime),
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
                    IfShowPromoCodeBox = true,
                    PromoPrice = 0,
                    TotalPrice = (float)currBooking.TotalPay,
                    DiscountTourists = currBooking.DiscountTourist,
                    DiscountPercent = (float)currBooking.DiscountPercent,
                    DiscountValue = (int)currBooking.DiscountValue,
                    MinTouristNum = tourObject.MinTouristNum,
                    MaxTouristNum = tourObject.MaxTouristNum,
                    TourID = tourId,
                    IsDataSaved = IsDataSaved,
                    BookingID = id,
                    PaymentModel = new PaymentModel()
                    {
                    BookID = id,
                    CardNumber = string.Empty,
                    ExpirationMonth = string.Empty,
                    ExpirationYear = string.Empty,
                    CVC = string.Empty,
                    PostalCode = string.Empty,
                    RememberMe = true,
                    PhoneNumber = string.Empty,
                    UserID = _security.GetCurrentUserId(),
                    Price = (float)currBooking.TotalPay,
#if DEBUG
                        StripePublishableKey = _StripePublicKeyTEST,
#else
                        StripePublishableKey = _StripePublicKey,
#endif
                        StripeToken = string.Empty,
                        StripeEmail = string.Empty,
                    TourName = currBooking.TourName,
                    },
                    TravellerInformationModel = new TravellerInformationModel()
                    {
                        FirstName = !traveler.FirstName.Contains("Guest") ?  traveler.FirstName:"",
                        LastName =  !traveler.LastName.Contains("Guest") ? traveler.LastName : "",
                        Email =     !traveler.Email.Contains("Guest") ? traveler.Email : "",
                        PhoneNumber =  traveler.Mobile,
                        IfWantNewsletter = true,
                    }                    
            };

                var sessionKey = string.Format("BookingID-{0}", id);
                HttpContext.Session[sessionKey] = model;

                //if GuestBooking we came here from a post->redirect we need to prefill travel Information form
                ViewBag.isGuestBooking = IsGuestBooking; 
                if (TempData.ContainsKey("BookingModel"))
                {                                       
                     ModelState.Merge((ModelStateDictionary)TempData["BookingModel"]);
                }
                if (TempData.ContainsKey("TravellerInformationModel"))
                {
                   model.TravellerInformationModel = TempData["TravellerInformationModel"] as TravellerInformationModel;
                }

                return View("Booking", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

      //  [Authorize]
        [HttpPost]
        public JsonResult ApplyPromoCode(int bookingID, string code)
        {
            Promo promo = null ;
            try
            {
                promo = _repositoryPromos.GetPromoCodeByComparing(code);
                if (promo == null)
                {
                    promo = new Promo();
                }
                else
                {
                    TempData.Remove("UserPromo");
                    UserPromo usedPromoCodes = _repositoryUsers.UserPromoGetById(_security.GetCurrentUserId(), promo.ID);
                    if (usedPromoCodes != null)
                    {
                        if (usedPromoCodes.isPromoUsed == true)
                            promo = new Promo();
                        else
                            TempData["UserPromo"] = usedPromoCodes;
                    }
                    else
                    {
                        usedPromoCodes = new UserPromo();
                        usedPromoCodes.UserID = _security.GetCurrentUserId();
                        usedPromoCodes.BookingID = bookingID;
                        usedPromoCodes.PromoID = promo.ID;
                        usedPromoCodes.isPromoUsed = false;
                        usedPromoCodes = _repositoryUsers.UserPromoCreateOrUpdate(usedPromoCodes);
                        TempData["UserPromo"] = usedPromoCodes;
                    }
                }

                //Update the tour booking's promoPrice and totalPrice
                var theBooking = _repositoryGuides.UserTourBookingGetByID(bookingID);
                if (theBooking == null)
                {
                    RedirectToAction("Message", "Notifications", new { title = "Booking Expired", text = "Sorry, The 15 minutes booking reservation time has been reached." });
                }

                var promoValue = promo.PromoValue;
                theBooking.PromoPrice = promoValue;
                theBooking.TotalPay = theBooking.SubTotal + theBooking.ServiceFee + promoValue ?? 0;
                if (theBooking.SubTotal > promo.MinValueToUse)//SubTotal below MinValueToUse will not taken into account
                {
                    var modifiedBooking = _repositoryGuides.UserTourBookingCreateOrUpdate(theBooking);
                    if (modifiedBooking == null)
                    {
                        RedirectToAction("InternalError", "Static", null);
                    }

                    var sessionKey = string.Format("BookingID-{0}", bookingID);
                    BookingModel BookingInfo = (BookingModel)HttpContext.Session[sessionKey];
                    BookingInfo.TotalPrice = (float)theBooking.TotalPay;
                    HttpContext.Session[sessionKey] = BookingInfo;
                }

            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                //return ErrorStrings.DatabaseError;
            }

            return Json(promo, JsonRequestBehavior.DenyGet);
        }

        public ActionResult DashBoard()
        {
            try
            {
                int totalCount = 0;
                var dashboardMsgs = GetReceivedMsgs(1, _ListViewPageSize, UserMsgStatus.All, out totalCount);
                var msgCount = dashboardMsgs.Count();
                var model = new DashBoardModel()
                {
                    Msgs = dashboardMsgs,
                    MessageCount = totalCount,
                    NextPageNo = msgCount == _ListViewPageSize ? 2 : 0,
                };
                return View("DashBoard", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult DashboardMsgList(int page)
        {
            try
            {
                int totalCount = 0;
                var model = GetReceivedMsgs(page, _ListViewPageSize, UserMsgStatus.All, out totalCount);
                var html = RenderRazorViewToString("DashBoardMsgList", model);
                var nextPageNo = 0;
                if (model != null && model.Count == _ListViewPageSize)
                {
                    nextPageNo = page + 1;
                }
                return Json(new
                {
                    Result = true,
                    Data = html,
                    NextPageNo = nextPageNo,
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    Result = false,
                    Data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Guider(int id)
        {
            try
            {
                var guider = _repositoryUsers.GetByIdOrNull(id);
                List<Tour> tourList;

                List<TourPicture> tourPictureList;
                List<TourSchedule> tourScheduleList;
                List<TourVendorPromo> tourVendorPromoList;
                List<UserTourReview> userTourReviewList;
                List<User> userList;
                RepositoryBigQueries.TourGetAllSimpleInfoByUserID(
                            id,
                            out tourList,
                            out tourPictureList,
                            out tourScheduleList,
                            out tourVendorPromoList,
                            out userTourReviewList,
                            out userList
                            );
                tourList.ForEach(tour => {
                    var picture = tourPictureList.Where(x => x.TourID == tour.ID).OrderBy(x => x.ID).FirstOrDefault();
                    tour.ImagePath = picture != null ? picture.RelativePath : string.Empty;
                    var schedulerPrice = tourScheduleList.Where(x => x.TourID == tour.ID).FirstOrDefault();
                    tour.SugRetailPrice = schedulerPrice != null ? schedulerPrice.SugRetailPrice ?? 0 : 0;

                    //tour.NowPrice = CalculateVendorPromotedPrice(tour.SugRetailPrice, tourVendorPromoSubset);
                    tour.NowPrice = schedulerPrice != null ? schedulerPrice.SugRetailPrice ?? 0 : 0;

                });

                //Only show ACTIVE tours on the guide profile page. 
                //Status = 0 Tour Creation Incompleted.
                //Status = 1 Tour Creation Completed.
                //Status = 2 Tour Active.
                //Status = 3 Tour Inactive.
                //Status = 4 Tour Delete.
                //Status = 5 Tour Published.
                List<TourListingInfo> listings = tourList.Where(x => x.Status == (byte)TourStatus.Published).Select(x => new TourListingInfo()
                {
                    TourId = x.ID,
                    TourName = x.Name,
                    SugRetailPrice = x.SugRetailPrice,
                    //NowPrice = x.NowPrice,
                    NowPrice = CalculateVendorPromotedPrice(x.SugRetailPrice, tourVendorPromoList.Where(t => t.TourID == x.ID)),
                    ImageUrl = string.IsNullOrEmpty(x.ImagePath) ? string.Empty : Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.ImagePath),
                    PerPersonOrGroup = BookingTypeTranslation.GetTranslationOf((TourBookingType)x.BookingType),
                    ReviewCount = x.ReviewCount,
                    ReviewAverageScore = x.ReviewAverageScore,
                }).ToList();
                //listings.ForEach(tour =>
                //{
                //    var tourVendorPromoSubset = tourVendorPromoList.Where(x => x.TourID == tour.TourId);
                //    tour.NowPrice = CalculateVendorPromotedPrice(tour.SugRetailPrice, tourVendorPromoSubset);
                //});
                double totalGuiderScore = 0;
                int reviewCount = 0;
                userTourReviewList.ForEach(review =>
                {
                    var user = userList.Where(x => x.ID == review.UserID).SingleOrDefault();
                    if (user != null)
                    {
                        review.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
                        review.UserAvatarUrl = user.AvatarUrl;
                    }
                    totalGuiderScore += review.AverageScore;
                    reviewCount++;
                });

                List<RecommendedReviews> reviews = userTourReviewList.Select(x => new RecommendedReviews()
                {
                    TourId = x.ID,
                    AvatarUrl = x.UserAvatarUrl,
                    UserName = x.UserName,
                    ReviewTime = x.ModifyTime.ToShortDateString(),
                    Comment = x.Comment,
                    AverageScore = x.AverageScore
                }).ToList();
                bool[] lanIds = new bool[] {
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                };
                tourList.ForEach(tourObject =>
                {
                    bool[] tourLanIds = new bool[] {
                        tourObject.IsEnglish,
                        tourObject.IsChineseMandarian,
                        tourObject.IsChineseCantonese,
                        tourObject.IsFrench,
                        tourObject.IsSpanish,
                        tourObject.IsGerman,
                        tourObject.IsPortuguese,
                        tourObject.IsItalian,
                        tourObject.IsRussian,
                        tourObject.IsKorean,
                        tourObject.IsJapanese,
                        tourObject.IsNorwegian,
                        tourObject.IsSwedish,
                        tourObject.IsDanish,
                    };
                    for (int i = 0, length = lanIds.Length; i < length; i++)
                    {
                        if (lanIds[i])
                            continue;
                        if (tourLanIds[i])
                        {
                            lanIds[i] = true;
                        }
                    }
                });
                var isEmailConfirmed = (guider.OpenSite == 0 && guider.IsConfirmed) || (guider.OpenSite > 0 && guider.IsEmailVerified4OpenID);

                var abbreviatedLastName = string.IsNullOrWhiteSpace(guider.LastName) ? "" : guider.LastName.Substring(0, 1) + ".";
                var firstNameAbbreviatedLastName = string.Format("{0} {1}", guider.FirstName, abbreviatedLastName);
                var model = new GuiderModel()
                {
                    GuiderID = id,
                    GuiderAvatarUrl = guider.AvatarUrl,
                    //GuiderName = string.Format("{0} {1}", guider.FirstName, guider.LastName),
                    GuiderName = firstNameAbbreviatedLastName,
                    GuiderLocation = guider.Address,
                    GuiderFromTime = guider.ModifyTime,
                    IsEmailConfirmed = isEmailConfirmed,
                    IsPhoneConfirmed = guider.IsPhoneConfirmed,
                    Tours = listings,
                    Languages = GetTourLanguageString(lanIds),
                    TourCount = listings.Count,
                    Reviews = reviews,
                    ReviewCount = reviews.Count,
                    ReviewAverageScore = reviewCount == 0 ? 0 : totalGuiderScore / reviewCount,
                    GuiderIntroduction = guider.Bio,
                    GuiderVideoURL = guider.VideoPath,

                };
                return View("Guider", InitLayout(model));
            }

            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        public ActionResult EditReview(int id)
        {
            try
            {
                var editReview = _repositoryTourists.UserTourReviewGetByUserIdTourId(_security.GetCurrentUserId(), id);
                EditReviewModel model = new EditReviewModel();
                var IsReviewAdded = false;
                if (editReview != null)
                    IsReviewAdded = true;
                else
                    IsReviewAdded = false;
                if (editReview != null)
                {
                    model.Accuracy = editReview.Accuracy;
                    model.Communication = editReview.Communication;
                    model.Services = editReview.Services;
                    model.Knowledge = editReview.Knowledge;
                    model.Value = editReview.Value;
                    model.Comment = editReview.Comment;
                    model.IsReviewAdded = IsReviewAdded;
                }
                return View("EditReview", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public ActionResult EditReview(int id, EditReviewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("EditReview", InitLayout(model));
                }
                var userId = _security.GetCurrentUserId();
                var tourReview = _repositoryTourists.UserTourReviewGetByUserIdTourId(userId, id);
                if (tourReview == null)
                {
                    tourReview = new UserTourReview();
                }
                tourReview.UserID = userId;
                tourReview.TourID = id;
                tourReview.Accuracy = (byte)model.Accuracy;
                tourReview.Communication = (byte)model.Communication;
                tourReview.Services = (byte)model.Services;
                tourReview.Knowledge = (byte)model.Knowledge;
                tourReview.Value = (byte)model.Value;
                tourReview.AverageScore = ((float)(model.Accuracy + model.Communication + model.Services + model.Knowledge + model.Value)) / 5;
                //tourReview.Comment = model.Comment;  
                tourReview.Comment = model.Comment ?? string.Empty;
                tourReview.EnterTime = DateTime.UtcNow;
                tourReview.ModifyTime = DateTime.UtcNow;
                _repositoryTourists.UserTourReviewCreateOrUpdate(tourReview);

                _repositoryGuides.TourUpdateReviewInfoByTourId(id);
                return RedirectToAction("Tourist");

            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        public ActionResult Share()
        {
            try
            {
                var model = new ShareModel();
                return View("Share", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [Authorize]
        public ActionResult TChat(int id)
        {
            try
            {
                var peer = _repositoryUsers.GetByIdOrNull(id);
                var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                var model = new TChatModel()
                {
                    PeerID = peer.ID,
                    PeerAvatarUrl = peer.AvatarUrl,
                    PeerUserName = peer.ShowName,
                    PeerScore = (int)_repositoryGuides.TourGetAverageScoreForGuider(id),
                    PeerReviewCount = _repositoryGuides.TourGetTotalReviewCountForGuider(id),
                    PeerLocation = peer.Address,
                    PeerRegDate = peer.EnterTime.ToShortDateString(),
                    PeerRole = peer.RealRole,
                    UserAvatarUrl = user.AvatarUrl,
                    UserID = user.ID,

                };
                return View("TChat", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult TChatSendMsg(int id, string msg)
        {
            try
            {
                int requestUserId = _security.GetCurrentUserId();
                if (requestUserId == id)
                {
                    return Json(new
                    {
                        Result = false,
                        Data = "Can not send message to yourself."
                    }, JsonRequestBehavior.DenyGet);
                }
                _repositoryUsers.UserMsgCreateOrUpdate(new UserMsg()
                {
                    FromUserID = requestUserId,
                    ToUserID = id,
                    Message = msg,
                    EnterTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                });
                return Json(new
                {
                    Result = true,
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
        [Authorize]
        [HttpPost]
        public JsonResult TChatGetMsgInPage(int id, int end, int page)
        {
            try
            {
                int requestUserId = _security.GetCurrentUserId();
                if (requestUserId == id)
                {
                    return Json(new
                    {
                        Result = false,
                        Data = "No message for yourself."
                    }, JsonRequestBehavior.DenyGet);
                }
                var msgs = _repositoryUsers.UserMsgGetAllByUserIDs(requestUserId, id, end, page, _ListViewPageSizeMsg).ToList();
                var peerUser = _repositoryUsers.GetByIdOrNull(id);
                int nextPageNo = msgs.Count() >= _ListViewPageSizeMsg ? page + 1 : 0;
                var model = new TChatListModel()
                {
                    Msgs = msgs.OrderBy(x => x.EnterTime)
                    .Select(x => new TChatMsg()
                    {
                        Date = x.EnterTime.ToString(TimeHelper.DefaultDateFormat),
                        Msg = x.Message,
                        IsFromMe = x.FromUserID == requestUserId,
                        PeerUserAvatarUrl = peerUser.AvatarUrl
                    })
                    .GroupBy(x => x.Date).ToList(),

                };
                var html = RenderRazorViewToString("TChatList", model);
                int endMsgId = 0;
                if (page == 1)
                {
                    var orderMsgs = msgs.OrderBy(x => x.EnterTime);
                    var endMsg = orderMsgs.LastOrDefault();
                    endMsgId = endMsg == null ? 0 : endMsg.ID;
                }
                return Json(new
                {
                    Result = true,
                    Data = html,
                    NextPageNo = nextPageNo,
                    EndId = endMsgId,
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    Result = false,
                    Data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult Tourist()
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                var user = _repositoryUsers.GetByIdOrNull(userId);
                List<UserWish> wishList;
                List<UserTourBooking> userTourBookingList;
                List<Tour> tourList;
                List<TourPicture> tourPictureList;
                List<TourSchedule> tourScheduleList;
                List<TourVendorPromo> tourVendorPromoList;
                List<TourExtra> extraList;
                List<UserTourReview> userTourReviewList;
                RepositoryBigQueries.TourGetAllTouristInfoByUserID(
                    userId,
                    out wishList,
                    out userTourBookingList,
                    out tourList,
                    out tourPictureList,
                    out tourScheduleList,
                    out tourVendorPromoList,
                    out extraList,
                    out userTourReviewList
                    );
                List<TourWishInfo> wishes = wishList.Select(x => new TourWishInfo()
                {
                    TourId = x.TourID,
                    TourName = x.TourName,
                    SugRetailPrice = x.SugRetailPrice,
                    NowPrice = x.NowPrice,
                    ImageUrl = Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.TourImgPath),
                    PerPersonOrGroup = BookingTypeTranslation.GetTranslationOf((TourBookingType)x.BookingType),
                }).ToList();
                wishes.ForEach(wish => {
                    var tour = tourList.Where(x => x.ID == wish.TourId).SingleOrDefault();
                    if (tour == null)
                        return;
                    wish.ReviewCount = tour.ReviewCount;
                    wish.ReviewAverageScore = tour.ReviewAverageScore;
                    var tourVendorPromoSubset = tourVendorPromoList.Where(x => x.TourID == wish.TourId);
                    wish.NowPrice = CalculateVendorPromotedPrice(wish.SugRetailPrice, tourVendorPromoSubset);
                });

                var userReviews = _repositoryTourists.UserTourReviewGetAllByUserID(userId);
                userTourBookingList.ForEach(booking =>
                {
                    var userReview = userReviews.Where(x => x.TourID == booking.TourID).FirstOrDefault();
                    if (userReview != null)
                    {
                        booking.IsReviewed = true;
                    }
                    else
                    {
                        booking.IsReviewed = false;
                    }
                    var tour = tourList.Where(x => x.ID == booking.TourID).SingleOrDefault();
                    if (tour != null)
                    {
                        booking.TourBookingType = tour.BookingType;
                    }
                });

                var purchases = userTourBookingList.Select(x => new TourPurchase()
                {
                    BookingId = x.ID,
                    TourId = x.TourID,
                    TourName = x.TourName,
                    GuiderName = x.GuiderName,
                    ImageUrl = string.IsNullOrEmpty(x.TourImgPath) ? string.Empty : Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.TourImgPath),
                    Date = x.Calendar,
                    Time = x.Time,
                    Location = x.Location,
                    TravellerCount = x.Travellers,
                    SubTotal = x.SubTotal.ToString(),
                    Taxes = x.Taxes.ToString(),
                    //Discount=x.DiscountValue.ToString(),

                    DiscountPercent = (float)x.DiscountPercent,
                    DiscountTourists = x.DiscountTourist,
                    DiscountValue = (int)x.DiscountValue,
                    TotalPay = x.TotalPay.ToString(),
                    Extras = extraList.Where(y => x.ExtraIds.SplitToIntArray(",").Contains(y.ID))
                            .Select(z => new TourExtraInfo()
                            {
                                ID = z.ID,
                                Name = z.Name,
                                Price = z.Price,
                                Times = z.Time,
                                TimeType = (TourTimeType)z.TimeType
                            }).ToList(),
                    IsReviewAdded = x.IsReviewed,
                    BookingType = x.TourBookingType,
                    ServiceFee = (float)x.ServiceFee,
                    PromoPrice = (float)x.PromoPrice,
                }).ToList();
                var model = new TouristModel()
                {
                    UserName = user.ShowName,
                    AvatarUrl = user.AvatarUrl,
                    Credits = user.Credits,
                    IsEmailConfirmed = (user.RealOpenSite > 0 && user.IsEmailVerified4OpenID) || (user.RealOpenSite == 0 && user.IsConfirmed),
                    IsPhoneConfirmed = user.IsPhoneConfirmed,
                    Wishes = wishes,
                    WishesCount = wishes.Count,
                    Purchases = purchases,
                    PurchasesCount = purchases.Count,
                };
                return View("Tourist", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public JsonResult SetWish(int tourId, int save)
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                if (userId <= 0)
                {
                    return Json(new
                    {
                        result = false,
                        message = "User not logoned.",
                    }, JsonRequestBehavior.DenyGet);
                }
                if (save == 0)
                {
                    _repositoryTourists.UserWishDeleteByUserIdTourId(userId, tourId);
                    return Json(new
                    {
                        result = true,
                    }, JsonRequestBehavior.DenyGet);
                }
                var userWish = _repositoryTourists.UserWishGetByUserIdTourId(userId, tourId);
                if (userWish == null)
                {
                    userWish = new UserWish()
                    {
                        ID = 0,
                        UserID = userId,
                        TourID = tourId,
                        EnterTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow,
                    };
                }
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
                if (tourObject == null)
                    return Json(new
                    {
                        result = false,
                        message = "Tour not found",
                    }, JsonRequestBehavior.DenyGet);

                userWish.TourName = tourObject.Name;
                userWish.SugRetailPrice = tourScheduleObject.SugRetailPrice ?? 0;
                userWish.NowPrice = tourScheduleObject.SugRetailPrice ?? 0;
                userWish.TourImgPath = tourPictureList[0].RelativePath;
                userWish.BookingType = tourObject.BookingType;
                _repositoryTourists.UserWishCreateOrUpdate(userWish);
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
                    message = "SetWish failed with exception!",
                }, JsonRequestBehavior.DenyGet);
            }
        }


        //
        // GET: /Account/LogOn
        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult 
            LogOnPopUp(string returnUrl, Nullable<int> role)
        {
            
            var model = new LogOnModel
            {
                SelectedPage = LayoutSelectedPage.Account,
                ReturnUrl = returnUrl,
                Lan = GetCurrentLanguage(),
                WeChatUrl = new WeChatHelper().GetLoginUrl(role ?? 1),
                Role = role ?? 1,
            };

            return View(InitLayout(model));
        }

        public ActionResult ContactUsPop(string nameTour)
        {
            try
            {
                var model = new ContactModel()
                {
                    TopicOptions = ContactTypeTranslation.Translations.ToSelectList(x => x.TypeString, x => x.Code.ToString()),
                    Topic = (int)ContactType.Feedback,
                    Name = string.Empty,
                    Email = string.Empty,
                    Phone = string.Empty,
                    Comment = string.Empty,
                };

                ViewBag.nameTour = nameTour;
                return View("ContactUsPop", InitLayout(model));

            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
















        [Route("Contact-Us", Order = 1)]
        //[Route("Tourist/Contact", Order = 2)]
        public ActionResult ContactUs()
        {
            try
            {
                var model = new ContactModel()
                {
                    TopicOptions = ContactTypeTranslation.Translations.ToSelectList(x => x.TypeString, x => x.Code.ToString()),
                    Topic = (int)ContactType.Feedback,
                    Name = string.Empty,
                    Email = string.Empty,
                    Phone = string.Empty,
                    Comment = string.Empty,
                };
                return View("ContactUs", InitLayout(model));

            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        //[Authorize]
        [HttpPost]
        [Route("Contact-Us", Order = 1)]
        public ActionResult Contact(ContactModel model)
        {
            try
            {
                ValidContactModel(model);
                if (ModelState.IsValid)
                {
                    var emailToCSR = new EmailUserContactCSR()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone,
                        Comment = model.Comment,
                        Topic = ContactTypeTranslation.Translations[model.Topic].TypeString,
                        UserName = model.Name,
                    };
                    _commands.SendUserContactCSREmail(StaticSiteConfiguration.CSREmailAddress, emailToCSR);
                    _commands.SendUserContactCSREmail(model.Email, emailToCSR);
                    return RedirectToAction("ContactDone");
                }
                model.TopicOptions = ContactTypeTranslation.Translations.ToSelectList(x => x.TypeString, x => x.Code.ToString());
                return View("ContactUs", InitLayout(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private void ValidContactModel(ContactModel model)
        {

        }
        public ActionResult ContactDone()
        {
            try
            {
                return View("ContactDone", InitLayout(new Layout()));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        private List<string> GetTourLanguageString(bool[] lanIds)
        {
            var languages = new List<string>();
            for (int i = 0; i < lanIds.Length; i++)
            {
                if (lanIds[i])
                    languages.Add(LanguageTranslation.Translations[i + 1].Language);
            }
            return languages;
        }

        [Route("promotion-code-20-off", Order = 1)]
        [Route("promotion", Order = 2)]
        public ActionResult PromotionLanding()
        {
            try
            {
                var model = new PromotionLandingModel()
                {
                    FullName = string.Empty,
                    Email = string.Empty
                };
                return View("PromotionLanding", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        [HttpPost]
        public ActionResult PromotionLandingProcess(PromotionLandingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailChimpServices.SubscribeToMailChimpList(model.Email, model.FullName);

                    return RedirectToAction("PromotionCode", "Tourist", null);
                }
                return View("PromotionLanding", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }




        [Route("promotion-code")]
        public ActionResult PromotionCode()
        {
            try
            {
                return View("PromotionCode", InitLayout(new Layout()));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        //[Authorize]
        [HttpPost]
        public JsonResult BookTimeDropDownList(int id, string date)
        {
            try
            {
                var bookTime = string.Empty;
                //var tourScheduleObject = _repositoryGuides.TourScheduleGetByTourID(id);
                //var userTourBookingList = _repositoryGuides.UserTourBookingGetByTourIDDate(id, date).ToList();
                Tour tourObject = null;
                List<TourExclusion> tourExclusionList = null;
                List<TourInclusion> tourInclusionList = null;
                List<TourPicture> tourPictureList = null;
                List<TourExtra> tourExtraList = null;
                List<TourPriceBreakdown> tourPriceBreakdownList = null;
                TourSchedule tourScheduleObject = null;
                List<TourVendorPromo> tourVendorPromoList = null;
                List<GuiderExcludedDates> guiderExcludedDatesList = null;

                List<UserTourBooking> userTourBookingList = null;
                //List<UserTourReview> userTourReviewList = null;

                List<User> userList = null;
                var userID = _security.GetCurrentUserId();
                RepositoryBigQueries.TourGetAllInfoByUserIDTourIDCalendar(userID, id, date, out tourObject, out tourExclusionList, out tourInclusionList, out tourPictureList,
                    out tourExtraList, out tourPriceBreakdownList, out tourScheduleObject, out tourVendorPromoList, out guiderExcludedDatesList, out userTourBookingList, out userList);

                bool isAvailable = false;
                List<SelectListItem> timeOptions = GetAvailTimeOptionOfTour(date, tourObject, userTourBookingList, tourScheduleObject, guiderExcludedDatesList, out bookTime, out isAvailable);

                var sugRetailPrice = tourScheduleObject.SugRetailPrice ?? 0;
                var tourVendorPromoSubset = tourVendorPromoList.Where(x => x.TourID == tourObject.ID);
                var requestedDateTime = DateTime.ParseExact(date, TimeHelper.DefaultDateFormat, CultureInfo.InvariantCulture);
                var nowPrice = CalculateVendorPromotedPriceOnDate(sugRetailPrice, tourVendorPromoSubset, requestedDateTime);

                var model = new BookTimeDropDownListModel
                {
                    BookTime = bookTime,
                    BookTimeOptions = timeOptions,
                };
                var html = RenderRazorViewToString("BookTimeDropDownList", model);
                return Json(new
                {
                    Result = true,
                    Data = html,
                    IsAvailable = isAvailable,
                    NowPrice = nowPrice
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    Result = false,
                    Data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult TourReviewList(int id, int page)
        {
            try
            {
                var userTourReviewList = _repositoryTourists.UserTourReviewGetInPageByTourId(id, page, _ListViewPageSize).ToList();
                List<RecommendedReviews> model = userTourReviewList.Select(x => new RecommendedReviews()
                {
                    TourId = x.TourID,
                    AvatarUrl = x.UserAvatarUrl,
                    UserName = x.UserName,
                    ReviewTime = x.ModifyTime.ToShortDateString(),
                    Comment = x.Comment,
                    AverageScore = x.AverageScore,
                }).ToList();
                var html = RenderRazorViewToString("TourReviewList", model);
                var nextPageNo = 0;
                if (model != null && model.Count == _ListViewPageSize)
                {
                    nextPageNo = page + 1;
                }
                return Json(new
                {
                    Result = true,
                    Data = html,
                    NextPageNo = nextPageNo,
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    Result = false,
                    Data = "exception happened!"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        private List<SelectListItem> GetAvailTimeOptionOfTour(string date, Tour tourObject, List<UserTourBooking> userTourBookings,
            TourSchedule tourScheduleObject, List<GuiderExcludedDates> guiderExcludedDatesList, out string bookTime, out bool isAvail)
        {
            var toBookDate = DateTime.ParseExact(date, TimeHelper.DefaultDateFormat, CultureInfo.InvariantCulture);
            bookTime = string.Empty;
            List<SelectListItem> timeOptions = null;
            if (userTourBookings == null
                || tourScheduleObject == null
                || tourScheduleObject.BeginDate > toBookDate
                || toBookDate > tourScheduleObject.EndDate
                || (toBookDate - DateTime.Now).TotalHours < tourObject.MinHourAdvance
                )
            {
                bookTime = "Try a different date";
                timeOptions = new string[] { "Try a different date" }.ToSelectList(x => x, x => x);
                isAvail = false;
                return timeOptions;
            }
            foreach (var guiderExcludedDate in guiderExcludedDatesList)
            {
                var excludedDate = DateTime.ParseExact(guiderExcludedDate.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (toBookDate.Equals(excludedDate) && guiderExcludedDate.Status == 1)//Status == 1 means this excluded date is in effect
                {
                    bookTime = "Try a different date";
                    timeOptions = new string[] { "Try a different date" }.ToSelectList(x => x, x => x);
                    isAvail = false;
                    return timeOptions;
                }
            }
            bool isDayOfWeekOK = true;
            if (toBookDate.DayOfWeek == DayOfWeek.Monday && !tourScheduleObject.IsMonday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Tuesday && !tourScheduleObject.IsTuesday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Wednesday && !tourScheduleObject.IsWednesday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Thursday && !tourScheduleObject.IsThursday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Friday && !tourScheduleObject.IsFriday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Saturday && !tourScheduleObject.IsSaturday)
            {
                isDayOfWeekOK = false;
            }
            else if (toBookDate.DayOfWeek == DayOfWeek.Sunday && !tourScheduleObject.IsSunday)
            {
                isDayOfWeekOK = false;
            }
            if (!isDayOfWeekOK)
            {
                bookTime = "Try a different date";
                timeOptions = new string[] { "Try a different date" }.ToSelectList(x => x, x => x);
                isAvail = false;
                return timeOptions;
            }
            var bookedTimes = userTourBookings.Select(x => x.Time).ToArray(); //Removing booking reservation restrictions
            var availTimes = new List<string>() { tourScheduleObject.StartTime1, tourScheduleObject.StartTime2, tourScheduleObject.StartTime3,
                    tourScheduleObject.StartTime4, tourScheduleObject.StartTime5, tourScheduleObject.StartTime6
                }.Where(x => !string.IsNullOrEmpty(x))
                .Where(x => !bookedTimes.Contains(x)) //Removing booking reservation restrictions
                .ToArray();
            var nonConflictAvailTimes = new List<string>();
            if (availTimes.Count() <= 0)
            {
                bookTime = "Sold Out";
                timeOptions = new string[] { "Sold Out" }.ToSelectList(x => x, x => x);
                isAvail = false;
                return timeOptions;
            }
            availTimes.ForEach(time =>
            {
                var startTime = TimeHelper.ParseExactDateTime(date, time);
                var conflictedBook = userTourBookings.Where(x => x.CalendarStart <= startTime && x.CalendarEnd >= startTime).FirstOrDefault();
                if (conflictedBook != null)
                {
                    return;
                }
                var duration = TourHelper.GetDurationTimeSpan(tourObject.Duration, tourObject.DurationTimeType);
                var endTime = startTime.Add(duration);
                conflictedBook = userTourBookings.Where(x => x.CalendarStart <= endTime && x.CalendarEnd >= endTime).FirstOrDefault();
                if (conflictedBook != null)
                {
                    return;
                }
                nonConflictAvailTimes.Add(time);
            });
            if (nonConflictAvailTimes.Count() <= 0)
            {
                bookTime = "Not available";
                timeOptions = new string[] { "Not available" }.ToSelectList(x => x, x => x);
                isAvail = false;
                return timeOptions;
            }
            bookTime = nonConflictAvailTimes[0];
            timeOptions = nonConflictAvailTimes.ToSelectList(x => x, x => x);
            isAvail = true;
            return timeOptions;
        }

        //[DonutOutputCache(CacheProfile = "OneDay")]
        //[Authorize]
        public ActionResult Tour(int id, string calendar)
        {
            try
            {


                if (string.IsNullOrEmpty(calendar))
                {
                    calendar = DateTime.UtcNow.ToString(MVCSite.Common.TimeHelper.DefaultDateFormat);
                    //_logger.LogError("string.IsNullOrEmpty(calendar)");
                    //return RedirectToAction("InternalError", "Static", null);                
                }
                if ( TempData.ContainsKey("TourModelState"))
                {
                }


                Tour tourObject = null;
                List<TourExclusion> tourExclusionList = null;
                List<TourInclusion> tourInclusionList = null;
                List<TourPicture> tourPictureList = null;
                List<TourExtra> tourExtraList = null;
                List<TourPriceBreakdown> tourPriceBreakdownList = null;
                TourSchedule tourScheduleObject = null;
                List<TourVendorPromo> tourVendorPromoList = null;
                List<GuiderExcludedDates> guiderExcludedDatesList = null;

                List<UserTourBooking> userTourBookingList = null;
                List<UserTourReview> userTourReviewList = null;
                List<User> userList = null;
                //var userID = _security.GetCurrentUserId();
                var theTour = _repositoryGuides.TourGetByID(id);
                var guideId = theTour.UserID;
                RepositoryBigQueries.TourGetAllInfoByUserIDTourIDCalendar(guideId, id, calendar, out tourObject, out tourExclusionList, out tourInclusionList, out tourPictureList,
                    out tourExtraList, out tourPriceBreakdownList, out tourScheduleObject, out tourVendorPromoList, out guiderExcludedDatesList, out userTourBookingList, out userList);

                if (tourObject == null)
                {
                    _logger.LogError("tourObject == null");
                    return RedirectToAction("InternalError", "Static", null);
                }
                bool[] lanIds = new bool[] {
                        tourObject.IsEnglish,
                        tourObject.IsChineseMandarian,
                        tourObject.IsChineseCantonese,
                        tourObject.IsFrench,
                        tourObject.IsSpanish,
                        tourObject.IsGerman,
                        tourObject.IsPortuguese,
                        tourObject.IsItalian,
                        tourObject.IsRussian,
                        tourObject.IsKorean,
                        tourObject.IsJapanese,
                        tourObject.IsNorwegian,
                        tourObject.IsSwedish,
                        tourObject.IsDanish,
                };
                List<string> languages = GetTourLanguageString(lanIds);
                var bookTime = string.Empty;
                bool isAvailable = false;
                List<SelectListItem> timeOptions = GetAvailTimeOptionOfTour(calendar, tourObject, userTourBookingList, tourScheduleObject, guiderExcludedDatesList, out bookTime, out isAvailable);
                var tourCity = _repositoryCities.GetCityByIdInDB(tourObject.TourCityID);

                var pagedReviewList = _repositoryTourists.UserTourReviewGetInPageByTourId(id, 1, _ListViewPageSize);
                var totalReviewMsgCount = pagedReviewList.TotalCount;
                userTourReviewList = pagedReviewList.ToList();
                userTourReviewList.ForEach(review =>
                {
                    var reviewUser = userList.Where(x => x.ID == review.UserID).SingleOrDefault();
                    if (reviewUser == null)
                        return;
                    review.UserAvatarUrl = reviewUser.AvatarUrl;
                    review.UserName = reviewUser.ShowName;
                    //review.UserName = string.Format("{0} {1}", reviewUser.FirstName, reviewUser.LastName);
                });


                List<RecommendedReviews> reviews = userTourReviewList.Select(x => new RecommendedReviews()
                {
                    TourId = x.TourID,
                    AvatarUrl = x.UserAvatarUrl,
                    UserName = x.UserName,
                    ReviewTime = x.ModifyTime.ToShortDateString(),
                    Comment = x.Comment,
                    AverageScore = x.AverageScore,
                }).ToList();
                int nextReviewPageNo = 0;
                if (userTourReviewList.Count == _ListViewPageSize)
                {
                    nextReviewPageNo = 2;
                }
                var userWish = _repositoryTourists.UserWishGetByUserIdTourId(_security.GetCurrentUserId(), id);
                var IsDataSaved = true;
                if (userWish == null)
                {
                    IsDataSaved = false;
                }
                var guider = _repositoryUsers.GetByIdOrNull(tourObject.UserID);
                var abbreviatedLastName = string.IsNullOrWhiteSpace(guider.LastName) ? "" : guider.LastName.Substring(0, 1) + ".";
                var firstNameAbbreviatedLastName = string.Format("{0} {1}", guider.FirstName, abbreviatedLastName);

                var sugRetailPrice = tourScheduleObject.SugRetailPrice ?? 0;
                var tourVendorPromoSubset = tourVendorPromoList.Where(x => x.TourID == tourObject.ID);
                var nowPrice = CalculateVendorPromotedPrice(sugRetailPrice, tourVendorPromoSubset);
                var endDate = tourScheduleObject.EndDate.AddHours(-tourObject.MinHourAdvance);
                if (endDate <= tourScheduleObject.BeginDate)
                {
                    endDate = tourScheduleObject.BeginDate;
                }
                if (endDate == DateTime.Today)
                {
                    guiderExcludedDatesList.Add(new GuiderExcludedDates() { Date = DateTime.Today.ToString("yyyyMMdd") });
                }
                var guiderExcludedDatesListStr = string.Empty;
                guiderExcludedDatesList.ForEach(x =>
                {
                    string yyyy = x.Date.Substring(0, 4);
                    string MM = x.Date.Substring(4, 2);
                    string dd = x.Date.Substring(6, 2);
                    string date = string.Format("{0}-{1}-{2}", MM, dd, yyyy);
                    guiderExcludedDatesListStr += date + "|";
                });
                var model = new TourModel
                {
                    IsMember = false,
                    IsSignedIn = _security.IsCurrentUserSignedIn(),
                    SelectedPage = LayoutSelectedPage.Account,
                    TourID = tourObject.ID,
                    GuiderID = tourObject.UserID,
                    Name = tourObject.Name,
                    ImageUrls = tourPictureList.Select(x => Path.Combine(StaticSiteConfiguration.ImageServerUrl, x.RelativePath)).ToList(),
                    CoverImagePath = tourPictureList.FirstOrDefault() != null ? tourPictureList.First().RelativePath : string.Empty,
                    Inclusions = tourInclusionList.Select(x => x.Name).ToList(),
                    Exclusions = tourExclusionList.Select(x => x.Name).ToList(),
                    Extras = tourExtraList?.Select(x => new TourExtraInfo()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Price = x.Price,
                        Times = x.Time,
                        TimeType = (TourTimeType)x.TimeType
                    }).ToList(),
                    IfShowExtras = tourExtraList?.Where(x => x.Name.Trim() != "").Count<TourExtra>() > 0,
                    TourPriceBreakdowns = tourPriceBreakdownList?.Select(x => new TourPriceBreakdownModel()
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
                    //Only retrieve rules valid for MinTouristNum and MaxTouristNum
                    .Where<TourPriceBreakdownModel>(
                        x => (x.EndPoint1 >= tourObject.MinTouristNum && x.EndPoint2 <= tourObject.MaxTouristNum) ||
                        (x.EndPoint1 >= tourObject.MinTouristNum && x.EndPoint1 <= tourObject.MaxTouristNum) ||
                        (x.EndPoint2 >= tourObject.MinTouristNum && x.EndPoint2 <= tourObject.MaxTouristNum)
                        )
                    .ToList(),
                    Overview = tourObject.Overview,
                    Itinerary = tourObject.Itinerary,
                    MinTouristNum = tourObject.MinTouristNum,
                    MaxTouristNum = tourObject.MaxTouristNum,
                    MinHourAdvance = tourObject.MinHourAdvance,

                    Duration = tourObject.Duration,
                    DurationTimeType = tourObject.DurationTimeType,
                    DurationString = TourHelper.GetDurationTimeString(tourObject.Duration, tourObject.DurationTimeType),
                    TourLocation = tourCity != null ? tourCity.UniqueCityName : string.Empty,
                    TourLocationSimple = tourCity != null ? tourCity.Name : string.Empty,
                    MeetupLocation = tourObject.MeetupLocation,
                    //GuiderName = guider.ShowName,
                    GuiderName = firstNameAbbreviatedLastName,
                    GuiderIntro = guider.Bio,
                    GuiderFromTime = guider.EnterTime.ToString(TimeHelper.DefaultDateFormat),
                    GuiderAvatarUrl = guider.AvatarUrl,
                    Language = languages.Aggregate((prev, next) => prev + " &<br/> " + next),
                    BookDate = calendar.Replace("/", "-"),//MM-dd-yyyy
                    TravellerCount = 1,
                    //TravellerCountOptions = GetIntArraySelectListItems(1, tourObject.MaxTouristNum),
                    TravellerCountOptions = GetIntArraySelectListItems(tourObject.MinTouristNum, tourObject.MaxTouristNum),
                    BookTime = bookTime,
                    BookTimeOptions = timeOptions,
                    SugRetailPrice = sugRetailPrice,
                    NowPrice = nowPrice,
                    DiscountTourists = tourScheduleObject.DiscountTourists ?? 0,
                    DiscountPercent = (float)(tourScheduleObject.DiscountPercent ?? 0),
                    DiscountValue = tourScheduleObject.DiscountValue ?? 0,
                    Reviews = reviews,
                    NextReviewPageNo = nextReviewPageNo,
                    SubTotal = 0,
                    Total = 0,
                    ExtraIds = string.Empty,
                    Tax = 0,
                    TourReviewCount = totalReviewMsgCount,
                    TourReviewAverageScore = tourObject.ReviewAverageScore,
                    BookingType = tourObject.BookingType,
                    IsDataSaved = IsDataSaved,
                    IsAvailable = isAvailable,
                    GuiderExcludedDatesListStr = guiderExcludedDatesListStr,//["//MM-dd-yyyy", "//MM-dd-yyyy"]                    
                    BeginDate = GetFirstAvailableDate(tourObject.MinHourAdvance,
                        new List<string> { tourScheduleObject.StartTime1, tourScheduleObject.StartTime2, tourScheduleObject.StartTime3, tourScheduleObject.StartTime4, tourScheduleObject.StartTime5, tourScheduleObject.StartTime6 }).ToString("MM-dd-yyyy"),
                    EndDate = endDate.ToString("MM-dd-yyyy"),//MM-dd-yyyy
                    IsSunday = tourScheduleObject.IsSunday.ToString(),
                    IsMonday = tourScheduleObject.IsMonday.ToString(),
                    IsTuesday = tourScheduleObject.IsTuesday.ToString(),
                    IsWednesday = tourScheduleObject.IsWednesday.ToString(),
                    IsThursday = tourScheduleObject.IsThursday.ToString(),
                    IsFriday = tourScheduleObject.IsFriday.ToString(),
                    IsSaturday = tourScheduleObject.IsSaturday.ToString().ToString(),
                    IsGuestBooking = false,
                };

              
              

                return View("Tour", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }


        
        //Get the first available date this tour should be available, given the MinHourAdvance and the 6 TourStartTimes
        DateTime GetFirstAvailableDate(int minHourAdvance, List<string> startTimesStr)
        {            
           startTimesStr = startTimesStr.Where(x => x!="").ToList();

            List <DateTime> startTimes = new List<DateTime>();
            startTimesStr.ForEach(x =>
            {
                if (x != null)
                {
                    DateTime dt = new DateTime();
                    DateTime.TryParse(x, out dt);
                    startTimes.Add(dt);
                }
            });
            DateTime maxStartTime = startTimes.Max();
            
            DateTime minAdvanceTime = DateTime.Now;
            minAdvanceTime = minAdvanceTime.AddHours(minHourAdvance);
            DateTime startTime = new DateTime(minAdvanceTime.Year,minAdvanceTime.Month,minAdvanceTime.Day,maxStartTime.Hour, maxStartTime.Minute, maxStartTime.Second);


            TimeSpan timeSpan =  startTime-DateTime.Now;

            while (timeSpan < new TimeSpan(0,minHourAdvance,0,0))
            {
                
                minAdvanceTime = minAdvanceTime.AddHours(24);
                startTime = new DateTime(minAdvanceTime.Year, minAdvanceTime.Month, minAdvanceTime.Day, maxStartTime.Hour, maxStartTime.Minute, maxStartTime.Second);
                timeSpan = startTime - DateTime.Now;
            }

            return minAdvanceTime;            
        }


        

        [HttpPost]
        public ActionResult Tour(TourModel model)
        {
            try
            {
               
                var userID = _security.GetCurrentUserId();
                
                if (userID <= 0)
                {
                    userID= 0;
                    var user =  RegisterGuestUser() ;
                    userID = user.ID;                 
                }
                //1. Delete all non-paid bookings happened 15 minutes ago
                _repositoryGuides.UserTourBookingDeleteAllExpiredNoPay();

                var utb = _repositoryGuides.UserTourBookingGetByTourIDTime(userID, model.TourID, model.BookDate, model.BookTime);
                //2. If not null, it means that there already has 1 other tourist booked this tour less than 15 minutes ago
                if (utb != null)
                {
                    //return RedirectToAction("Message","Notifications", new { title = "Booking time NOT available",text="Sorry Someone else has booked this tour time,please choose another one and try again." });
                    return RedirectToAction("Message", "Notifications", new { title = "Booking Time Not Available", text = "Sorry, Someone is currently booking this tour time. Please wait 15 minutes to see if it becomes available or choose another time and try again." });
                }
                model.BookDate = model.BookDate.Replace("-", "/");//MM/dd/yyyy
                var start = TimeHelper.ParseExactDateTime(model.BookDate, model.BookTime);
                var duration = TourHelper.GetDurationTimeSpan(model.Duration, model.DurationTimeType);
                var serviceFee = model.SubTotal * 0.039;
                var booking = _repositoryGuides.UserTourBookingCreateOrUpdate(new UserTourBooking()
                {
                    UserID = userID,
                    TourID = model.TourID,
                    Calendar = model.BookDate,
                    Time = model.BookTime,
                    Travellers = model.TravellerCount,
                    EnterTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                    TourUserID = model.GuiderID,
                    TourName = model.Name,
                    CalendarStart = start,
                    CalendarEnd = start.Add(duration),
                    TourImgPath = model.CoverImagePath,
                    GuiderName = model.GuiderName,
                    Location = model.TourLocation,
                    Status = (byte)UserTourBookingStatus.Initial,
                    SubTotal = model.SubTotal,
                    Taxes = model.Tax,
                    DiscountValue = model.DiscountValue,
                    TotalPay = model.Total + serviceFee,
                    ExtraIds = model.ExtraIds ?? string.Empty,
                    DiscountTourist = (byte)model.DiscountTourists,
                    DiscountPercent = model.DiscountPercent,
                    ServiceFee = serviceFee
                });

                return RedirectToAction("Booking", new
                {
                    id = booking.ID,
                    tourId = booking.TourID,
                    userId = booking.UserID,
                    isGuestBooking = model.IsGuestBooking
                });
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

       
      //  [Authorize]
        [HttpPost]
        public ActionResult Payment(BookingModel model)
        {
            try
            {
                if (model.TravellerInformationModel!=null)
                     Validators.ValidEmailAndCheckDB(_repositoryUsers, ModelState, model.TravellerInformationModel.Email, "Email");
                if (!ModelState.IsValid)
                {                  
                    TempData.Add("BookingModel", ModelState);
                    TempData.Add("TravellerInformationModel", model.TravellerInformationModel);
                    return RedirectToAction("Booking", new { ID=model.BookingID ,TourID=model.TourID, UserID=model.UserID,IsGuestBooking=true});
                }

                //update user info if it was guest booking
                User user = _repositoryUsers.GetByIdOrNull(model.UserID);
                if (model.TravellerInformationModel != null)
                {
                    user.FirstName = model.TravellerInformationModel.FirstName;
                    user.LastName = model.TravellerInformationModel.LastName;
                    user.Email = model.TravellerInformationModel.Email;
                    user.Mobile = model.TravellerInformationModel.PhoneNumber;
                    _repositoryUsers.UserUpdate(user);
                }
                //Payment through Stripe
                var chargeOptions = new StripeChargeCreateOptions()
                {
                    //required
                    Amount = (int)model.PaymentModel.Price * 100,
                    Currency = "usd",
                    Source = new StripeSourceOptions() { TokenId = model.PaymentModel.StripeToken },
                    //optional
                    Description = string.Format("{0} for {1}", model.TourName, model.PaymentModel.StripeEmail),
                    ReceiptEmail = model.PaymentModel.StripeEmail
                };
                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.Create(chargeOptions);
                _repositoryGuides.UserTourBookingUpdateStatus(model.PaymentModel.BookID, (byte)UserTourBookingStatus.Paid);


                //Send Confirmation Emails
                var BookID = model.PaymentModel.BookID;
                var sessionKey = string.Format("BookingID-{0}", BookID);
                if (HttpContext.Session[sessionKey] == null)
                {
                    //eturn RedirectToAction("InternalError", "Static", null);
                    return RedirectToAction("Message", "Notifications",
                        new
                        {
                            title = "Booking Expired",
                            text = "Sorry, The 15 minutes booking reservation time has been reached."
                        });
                }

                BookingModel BookingInfo = (BookingModel)HttpContext.Session[sessionKey];

                List<BookingConfirmationTourExtraInfo> BookingConfirmationTourExtraInfoList =
                    new List<BookingConfirmationTourExtraInfo>();
                foreach (var extraInfo in BookingInfo.Extras)
                {
                    BookingConfirmationTourExtraInfo BookingConfirmationTourExtraInfo = new BookingConfirmationTourExtraInfo()
                    {
                        ID = extraInfo.ID,
                        Name = extraInfo.Name,
                        Price = extraInfo.Price,
                        Times = extraInfo.Times,
                        TimeType = extraInfo.TimeType
                    };
                    BookingConfirmationTourExtraInfoList.Add(BookingConfirmationTourExtraInfo);
                }
                BookingConfirmationModel bookingConfirmationModel = new BookingConfirmationModel()
                {
                    ConfirmationCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),

                    #region BookingInfo
                    TourID = BookingInfo.TourID,
                    TourUserID = BookingInfo.TourUserID, //TourUserID is the same as GuideID
                    BookingID = BookingInfo.BookingID,
                    City = BookingInfo.City,
                    ImageUrl = BookingInfo.ImageUrl,
                    TourName = BookingInfo.TourName,

                    TravelerName = string.Format("{0} {1}", user.FirstName, user.LastName),
                    TravelerEmail = user.Email,
                    TravelerPhoneAreaCode = user.PhoneAreaCode,
                    TravelerMobile = user.Mobile,


                    GuideName = BookingInfo.GuideName,
                    GuideID = BookingInfo.GuideID, //TourUserID is the same as GuideID
                    GuideAvatarPath = BookingInfo.GuideAvatarPath,
                    GuideEmail = BookingInfo.GuideEmail,
                    GuidePhoneAreaCode = BookingInfo.GuidePhoneAreaCode,
                    GuideMobile = BookingInfo.GuideMobile,

                    Date = BookingInfo.Date,
                    Time = BookingInfo.Time,
                    Location = BookingInfo.Location,
                    MeetupLocation = BookingInfo.MeetupLocation,
                    TourLocationSimple = BookingInfo.TourLocationSimple,
                    SubTotalPrice = BookingInfo.SubTotalPrice,
                    ServiceFee = BookingInfo.ServiceFee,
                    DiscountTourists = BookingInfo.DiscountTourists,
                    DiscountValue = BookingInfo.DiscountValue,
                    DiscountPercent = BookingInfo.DiscountPercent,
                    Taxes = BookingInfo.Taxes,
                    BookingType = BookingInfo.BookingType,
                    TotalPrice = BookingInfo.TotalPrice,
                    TotalTravelers = BookingInfo.TotalTravelers,
                    IsDataSaved = BookingInfo.IsDataSaved,
                    TotalCost = BookingInfo.TotalCost,
                    TourCost = (float)BookingInfo.TourCost,
                    VendorPromoTourCost = (float)BookingInfo.VendorPromoTourCost,
                    Extras = BookingConfirmationTourExtraInfoList,
                    TourPriceBreakdown = BookingInfo.TourPriceBreakdown,
                    TempPassword = Session["TempPassword"] as String ?? "",
                    
                    #endregion
                };

#if !DEBUG
                _commands.SendBookingConfirmationEmail(bookingConfirmationModel);
#endif


                //  if (model.TravellerInformationModel.FirstName!=null && model.TravellerInformationModel.IfWantNewsletter)
                //    MailChimpServices.SubscribeToMailChimpList(model.TravellerInformationModel.Email, string.Format("{0} {1}", model.TravellerInformationModel.FirstName, model.TravellerInformationModel.LastName));


                //return RedirectToAction("Index", "Tourist", null);
                if (TempData.ContainsKey("UserPromo"))
                {
                    UserPromo userPromo = TempData["UserPromo"] as UserPromo;
                    userPromo.isPromoUsed = true;
                    _repositoryUsers.UserPromoCreateOrUpdate(userPromo);
                    TempData.Remove("UserPromo");
                }
                return RedirectToAction("PaymentConfirmed", new { pn = stripeCharge.ReceiptNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return RedirectToAction("InternalError", "Static", null);
            }
        }


        private User RegisterGuestUser()
        {
          

                //LogonMode from = (LogonMode)model.From;
                var userBrowser = string.Empty;
                bool validateEmail = true;
              //  var userAgent = Request.UserAgent;
               // string originalEmail = model.Email;
                
            
              
                userBrowser = CheckUserAgentLength(Request.UserAgent);
                validateEmail = true;
               /// ValidRegisterEmailUserNamePassword(model.Email, model.FirstName, model.Password, model.NickName, validateEmail);
               
            
                    try
                    {
                        var userIp = RequestHelper.GetClientIpAddress(Request);
                        UserRole role = (UserRole)UserRole.Tourist;

                        string email = "Guest" + Guid.NewGuid().ToString().Substring(0, 8) + "@mail.com";
                        string password = Guid.NewGuid().ToString().Substring(0, 8);
                        Session.Add("TempPassword", password);
                        var page = _commands.Register(new UserRegInfo()
                        {
                            FirstName = "Guest",
                            LastName = "Guest",
                            Email = email,
                            Password = password,
                            Title = string.Empty,
                            AreaCode = string.Empty,
                            LocalCode = string.Empty,
                            RequireConfirmationToken = true,
                            UserIp = userIp,
                            UserBrowser = userBrowser,
                            NickName = "",
                            Role = (short)role,
                            Language = (byte)GetCurrentLanguage(),
                           
                        });


                //  var userGuid = SafeConvert.ToInt32(page.Parameters);


               return _repositoryUsers.GetByEmailOrNull(email);
               // return _repositoryUsers.ActivateUserByPassConfirmationToken(email);
                        //Autotically logon
                       // var returnModel = _commands.LogOn(model.Email, model.Password, true, model.ReturnUrl, userIp, userBrowser);
                       
                    
                    }
                    catch (SecurityException e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                        return null;
                    }
                
               
            
          


        }


      //  [Authorize]
        public ActionResult PaymentConfirmed(string pn)
        {
            try
            {
                var model = new PaymentConfirmedModel()
                {
                    BookingNo = pn,
                    TempPassword = Session["TempPassword"] as string?? "",


                    
                };
                Session.Remove("TempPassword");
                return View("PaymentConfirmed", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
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
        public double CalculateVendorPromotedPriceOnDate(double nowPrice, IEnumerable<TourVendorPromo> tourVendorPromoSubset, DateTime oneDate)
        {
            foreach (var vendorPromo in tourVendorPromoSubset)
            {
                if (vendorPromo.BeginDate.AddDays(-1) <= oneDate && oneDate <= vendorPromo.EndDate.AddDays(1))
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
        //Real Publish Action
        [ChildActionOnly]
        [HttpPost]
        public void TourSetStatus(int id, byte status)
        {
            try
            {
                _repositoryGuides.TourUpdateStatus(id, status); //Actual Status Update
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
            }
        }
    }
}
