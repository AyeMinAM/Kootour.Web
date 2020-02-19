using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Common;
using MVCSite.Biz.Interfaces;
using System.Security;
using MVCSite.Biz;
using MVCSite.DAC.Entities;
using MVCSite.ViewResource;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Instrumentation.Membership;
using System.IO;
using DevTrends.MvcDonutCaching;
using MVCSite.Web.Extensions;
using DotNetOpenAuth.OpenId.RelyingParty;
using MVCSite.DAC.Instrumentation;
using System.Web.Script.Serialization;
using MVCSite.Common;

using System.Text;
using MVCSite.DAC.Extensions;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.ApplicationBlock;
using MVCSite.DAC.Services;
using MVCSite.Web.Services;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;

namespace MVCSite.Web.Controllers
{
    [Authorize]
    public class AdminController : LayoutBase
    {
        private readonly IPublicCommands _commands;
        private readonly Func<IUserQueries> _queriesFactory;
        private readonly Func<IUserCommands> _commandsFactory;
        protected readonly IRepositoryGuides _repositoryGuides;
        protected readonly IRepositoryCities _repositoryCities;
        private readonly GuideService _guideService;

        private static string _SingleSignOnState = string.Empty;
        private static int _helloCountSinceLast = 0;
        private static int _timeSpanToSave = 180;//seconds
        private static DateTime _lastSaveTime = DateTime.UtcNow;


        public AdminController(
            ISecurity security, 
            IWebApplicationContext webContext,
            ISiteConfiguration configuration,
            IPublicCommands commands,
            Func<IUserQueries> queriesFactory,
            Func<IUserCommands> commandsFactory,
            IRepositoryUsers repositoryUsers,
            IRepositoryGuides repositoryGuides,
            IRepositoryCities repositoryCities,
            MVCSite.Common.ILogger logger
            )
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
            _commands = commands;
            _queriesFactory = queriesFactory;
            _commandsFactory = commandsFactory;
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



      

       

        [Authorize]
        [Route("admin/console/{*pathInfo}", Order=1)]
        public ActionResult Console()
        {
            try
            {
              if (IsNotAdmin())
                {
                    return RedirectToAction("InternalError", "Static", null);
                }
                //_repositoryUsers.UserUpdateRole(userId, UserRole.Guider);

                var model = new AdminUserAccountModel
                {
                    //users = userlist
                }; 
                return View(InitLayout(model));
                //return RedirectToAction("User", "Admin");
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        //public ActionResult User()
        //{
        //    //var userlist = _repositoryUsers.UserGetAll();

        //    ////return View("Manage", InitLayout(lists));
        //    var model = new AdminUserAccountModel
        //    {
        //        //users = userlist
        //    };
        //    model.PageTitle = "Admin Console | Kootour";
        //    return View(InitLayout(model));
        //    //return View("User");
        //}

        //public ActionResult Manage()
        //{
        //    var model = new AdminUserAccountModel
        //    {
        //        //users = userlist
        //    };
        //    model.PageTitle = "Admin Console | Kootour";
        //    return View(InitLayout(model));
        //    //return View("Manage");
        //}



        [Authorize]
        public ActionResult TourType(Nullable<int> id)
        {
            return _guideService.ShowTourType(id);
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
                }
                model.LanguageOptions = LanguageTranslation.Translations.Skip(1).ToSelectList(x => x.Language, x => x.Code.ToString());
                return View("TourType", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
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
            foreach (var type in types)
            {
                if (type)
                {
                    typeCount++;
                }
            }
            if (model.LanguageIDs == null || model.LanguageIDs.Length <= 0)
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
        public ActionResult BookingDetails(Nullable<int> id)
        {
            return _guideService.ShowBookingDetails(id);
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
            if (model.MinHourAdvance <= 0)
            {
                ModelState.AddModelError("MinHourAdvance", ValidationStrings.MinHourAdvance);
            }
            return;
        }
        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult SchedulerPrice(Nullable<int> id)
        {
            try
            {
                return _guideService.ShowSchedulerPrice(id);
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
            var tourPriceBreakdownsCount = model.TourPriceBreakdowns == null ? 0 : model.TourPriceBreakdowns.Count;
            for (int i = 0; i < allTourPriceBreakdowns.Count; i++)
            {
                if (allTourPriceBreakdowns[i]?.DiscountValue == 0 && allTourPriceBreakdowns[i]?.DiscountPercent == 0)
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
                            ModelState.AddModelError("TourPriceBreakdownExtraValidationMessage" + (i - tourPriceBreakdownsCount), GuideStrings.TourPriceBreakdownExtraValidationMessage + " " + (j + 1));
                            return;
                        }
                        //ModelState.AddModelError("TourPriceBreakdownExtraValidationMessage" + i, GuideStrings.TourPriceBreakdownExtraValidationMessage + " " + (j + 1));
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
                    }
                }
            }
            return;
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult Pictures(Nullable<int> id)
        {
            return _guideService.ShowPictures(id);
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
                    //return RedirectToAction("Console/Tour");
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
            if (tourPictures.Count < 3)
                ModelState.AddModelError("ValidateInfo", "Please upload at least 3 pictures for your tour");
            return;
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
                                status = (byte)TourStatus.Active,
                                NextActionName = "Console/Tour",
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
                                NextActionName = "Console/Tour",
                                NextControllerName = RouteData.Values["controller"],

                                DoneActionName = string.Format("Publish/{0}", model.TourID),
                                DoneControllerName = RouteData.Values["controller"]
                            });
                    }
                    //return RedirectToAction("Console/Tour");
                }
                return View("Publish", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }

        [HttpGet]
        public string GetUsers()
        {
            var list = _repositoryUsers.UserGetAll().ToList();
            foreach (var user in list)
            {
                var IsEmailVerified = (user.RealOpenSite > 0 && user.IsEmailVerified4OpenID) ||
                                      (user.RealOpenSite == 0 && user.IsConfirmed);
                user.IsConfirmed = IsEmailVerified;
            }
            string json = JsonConvert.SerializeObject(list, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return json;
        }

        [HttpGet]
        public string GetGuides()
        {
            var list = _repositoryUsers.GuideGetAll().ToList();
            foreach (var user in list)
            {
                var IsEmailVerified = (user.RealOpenSite > 0 && user.IsEmailVerified4OpenID) ||
                                      (user.RealOpenSite == 0 && user.IsConfirmed);
                user.IsConfirmed = IsEmailVerified;
            }
            string json = JsonConvert.SerializeObject(list, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return json;
        }
        [HttpGet]
        public string GetTravellers()
        {
            var list = _repositoryUsers.TravellerGetAll().ToList();
            foreach (var user in list)
            {
                var IsEmailVerified = (user.RealOpenSite > 0 && user.IsEmailVerified4OpenID) ||
                                      (user.RealOpenSite == 0 && user.IsConfirmed);
                user.IsConfirmed = IsEmailVerified;
            }
            string json = JsonConvert.SerializeObject(list, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return json;
        }
        [HttpGet]
        public string GetTours()
        {
            try
            {
                var tourlist = _repositoryGuides.TourGetAllRichInfo().ToList();

                string json = JsonConvert.SerializeObject(tourlist, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                return json;
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return "";
                //return RedirectToAction("InternalError", "Static", null);
            }
        }
        //Method to set tour status
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

        //Method to set tour IsFeatured status
        [HttpPost]
        public void TourSetFeaturedTour(int id, bool status)
        {
            try
            {
                _repositoryGuides.TourUpdateFeaturedTour(id, status); //Actual Status Update
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
            }
        }

        [HttpPost]//admin/UserSetFirstName/256
        public string UserSetBio(int id, string bio)
        {
            try
            {

                var user = _repositoryUsers.GetByIdOrNull(id);
                user.Bio = bio;


                user.ModifyTime = DateTime.UtcNow;
                _repositoryUsers.UserUpdate(user);


                string json = JsonConvert.SerializeObject(user, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                return json;
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return "";
            }
        }
    }
}
