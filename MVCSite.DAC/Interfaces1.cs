


//using WMath.Engine.DomainEntities;

using System;
using System.Collections.Generic;
using MVCSite.DAC.Entities;

namespace MVCSite.DAC.Interfaces
{
    public partial interface ISiteConfiguration
    {
        System.IO.DirectoryInfo UploadImagesFolder { set; get; }
        string DontReplyEmailAddress { set; get; }
        string AbuseReportEmailAddress { set; get; }
        string ContactUserEmailAddress { set; get; }
        MVCSite.DAC.Common.ValidationSettings UserName { set; get; }
        MVCSite.DAC.Common.ValidationSettings Password { set; get; }
        MVCSite.DAC.Common.ValidationSettings NickName { set; get; }
        System.Uri MyDealBagServiceURL { set; get; }
        int DefaultPageSize { set; get; }
        string ServerUrl { set; get; }
        bool JudgeCityByRequestUserIp { set; get; }
    }

    public partial interface IWebApplicationContext
    {
        System.Uri RequestUrl { get; }
        bool IsLocalUrl(string url);
        System.Web.Mvc.UrlHelper UrlHelper { get; }
        string ServerUrl { get; }
        string ServerUrlOrNull { get; }
        string UserIpAddress { get; }
        string GetCookie(string name);
        void SetCookie(string name, string value);
        void ValidateThatUserCanSignIn(MVCSite.DAC.Entities.User user, string password);
        bool IsCurrentUserSignedIn();
        string GetCurrentUserNameOrNull();
        string GetCurrentUserName();
        int? GetCurrentUserIdOrNull();
        int GetCurrentUserId();
    }

    public partial interface IRepository
    {
    }

    public partial interface IRepositoryCities
    {
        MVCSite.DAC.Entities.City GetCityByUniqueNameOrNullInDB(string name);
        MVCSite.DAC.Entities.City GetCityByUniqueCityNameOrNullInDB(string name);
        MVCSite.DAC.Entities.City GetCityByNameInUrlOrNullInDB(string name);
        MVCSite.DAC.Entities.City GetCityByUniqueCityNameOrNullInDBViaDashedNames(string name);
        MVCSite.DAC.Entities.City GetCityByIdInDB(int cityId);
        System.Collections.Generic.List<MVCSite.DAC.Entities.City> GetCityListByIdList(IEnumerable<int> cityList);
        MVCSite.DAC.Entities.Country CountryCreateOrGet(string country);
        MVCSite.DAC.Entities.City CityCreateOrGet(string name, string uniqueName, string nameInUrl, int countrId, int regionId);
        void CityUpdateIsInUseInfo(MVCSite.DAC.Entities.City current);
        MVCSite.DAC.Entities.Region RegionCreateOrGet(string region, int countryId);
        MVCSite.DAC.Entities.Country CountryCreateOrUpdate(MVCSite.DAC.Entities.Country key);
        System.Collections.Generic.List<System.Web.Mvc.SelectListItem> GetPhoneCodesSelectListItemCached();
        System.Collections.Generic.List<System.Web.Mvc.SelectListItem> GetCitiesSelectListItemCached();
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.City> GetAllCitiesInUseCached();
        MVCSite.DAC.Repositories.CitySelectorData GetCitySelectorDataCached();
        MVCSite.DAC.Repositories.CitySelectorData GetCitySelectorData();
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.Country> GetCountriesCached();
        MVCSite.DAC.Entities.Country GetCountryByNameCached(string countryName);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.City> GetAllCachedCitiesStartWith(int maxRows, int countryId, string startWith);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.City> GetAllCachedCitiesStartWith(int maxRows, string startWith);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.Country> GetAllCachedCountriesStartWith(int maxRows, string startWith);
        Country GetCountryById(int? id);
        int? GetCityIdByUrlName(string cityUrlName);
        MVCSite.DAC.Entities.City GetCityOrNull(int cityId);
        MVCSite.DAC.Entities.City GetCityByName(string name);
        MVCSite.DAC.Entities.City GetCityByNameOrNull(string name);
        MVCSite.DAC.Entities.City GetCityByNameInUrlOrNull(string name);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.City> GetCitiesThatStartsWith(string nameStartsWith, int maxRows);
    }

    public partial interface IRepositoryGuides
    {
        MVCSite.DAC.Entities.UserTourBooking UserTourBookingGetByID(int id);
        MVCSite.DAC.Entities.UserTourBooking UserTourBookingGetByTourIDTime(int userId, int tourId, string date, string time);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourBooking> UserTourBookingGetAllByTourUserIDStartEnd(int tourUserId, System.DateTime start, System.DateTime end);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourBooking> UserTourBookingGetAllByTourID(int tourId);
        int UserTourBookingGetCountByTourID(int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourBooking> UserTourBookingGetByTourIDDate(int tourId, string date);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourBooking> UserTourBookingGetAllByUserID(int userId);
        MVCSite.DAC.Entities.UserTourBooking UserTourBookingCreateOrUpdate(MVCSite.DAC.Entities.UserTourBooking key);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> GetAllFeaturedToursCached();
        //MVCSite.DAC.Entities.UserTourBooking UserTourBookingUpdateByID(MVCSite.DAC.Entities.UserTourBooking key);
        void TourUpdateFeaturedTour(int id, bool status);
        void UserTourBookingUpdateStatus(int id, byte status);
        void UserTourBookingDeleteAllExpiredNoPay();
        void UserTourBookingDeleteAll();
        double TourGetAverageScoreForGuider(int guideid);
        int TourGetTotalReviewCountForGuider(int guideid);
        System.Collections.Generic.List<int> TourGetAllOccupiedCityIds();
        MVCSite.DAC.Entities.Tour TourGetByID(int id);
        MVCSite.DAC.Entities.Tour TourGetByUserIDStatus(int userId, MVCSite.DAC.Entities.TourStatus status);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.Tour> TourGetAllByUserID(int userId);
        System.Linq.IQueryable<MVCSite.DAC.Entities.Tour> TourGetAllRichInfo();
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> TourGetAllByPage(int userId, int page, int pageSize);
        //MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> TourGetFeaturedByPage(int page, int pageSize);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.Tour> TourGetAllByIds(int[] tourIds);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> TourGetAllInCityByPage(MVCSite.DAC.Entities.SearchCriteria criteria, int page, int pageSize);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> TourGetAllBySearchModeByPage(SearchCriteria criteria, int page, int pageSize, byte searchMode);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.Tour> TourGetAllFeaturedCached();
        MVCSite.DAC.Entities.Tour TourCreateOrUpdate(MVCSite.DAC.Entities.Tour key);
        void TourDeleteByID(int tourId);
        MVCSite.DAC.Entities.Tour TourCloneByID(int tourId);
        void TourUpdateStatus(int id, byte status);
        void TourUpdateReviewInfoByTourId(int tourId);
        MVCSite.DAC.Entities.TourExtra TourExtraGetByID(int id);
        MVCSite.DAC.Entities.TourExtra TourExtraGetByTourIDSortNo(int tourId, byte sortNo);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourExtra> TourExtraGetAllByTourID(int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourExtra> TourExtraGetAllByIds(int[] tourExtraIds);
        MVCSite.DAC.Entities.TourExtra TourExtraCreateOrUpdate(MVCSite.DAC.Entities.TourExtra key);
        MVCSite.DAC.Entities.TourInclusion TourInclusionGetByID(int id);
        MVCSite.DAC.Entities.TourInclusion TourInclusionGetByTourIDSortNo(int tourId, byte sortNo);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourInclusion> TourInclusionGetAllByTourID(int tourId);
        MVCSite.DAC.Entities.TourInclusion TourInclusionCreateOrUpdate(MVCSite.DAC.Entities.TourInclusion key);

        void TourInclusionDeleteOld(int tourId, byte sortNo);
        MVCSite.DAC.Entities.TourExclusion TourExclusionGetByID(int id);
        MVCSite.DAC.Entities.TourExclusion TourExclusionGetByTourIDSortNo(int tourId, byte sortNo);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourExclusion> TourExclusionGetAllByTourID(int tourId);
        MVCSite.DAC.Entities.TourExclusion TourExclusionCreateOrUpdate(MVCSite.DAC.Entities.TourExclusion key);
        void TourExclusionDeleteOld(int tourId, byte sortNo);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourPriceBreakdown> TourPriceBreakdownGetAllByTourID(int tourId);
        MVCSite.DAC.Entities.TourPriceBreakdown TourPriceBreakdownGetByTourIDSortNo(int tourId, byte sortNo);

        MVCSite.DAC.Entities.TourPriceBreakdown TourPriceBreakdownCreateOrUpdate(MVCSite.DAC.Entities.TourPriceBreakdown key);
        void TourPriceBreakdownDeleteOld(int tourId, byte sortNo);

        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourVendorPromo> TourVendorPromoGetAllByTourID(int tourId);
        MVCSite.DAC.Entities.TourVendorPromo TourVendorPromoGetByTourIDSortNo(int tourId, byte sortNo);

        MVCSite.DAC.Entities.TourVendorPromo TourVendorPromoCreateOrUpdate(MVCSite.DAC.Entities.TourVendorPromo key);
        void TourVendorPromoDeleteOld(int tourId, byte sortNo);
        MVCSite.DAC.Entities.TourPicture TourPictureGetByID(int id);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.TourPicture> TourPictureGetAllByTourID(int tourId);
        int TourPictureGetTotalCountOfTourId(int tourId);
        MVCSite.DAC.Entities.TourPicture TourPictureCreateOrUpdate(MVCSite.DAC.Entities.TourPicture key);
        void TourPictureDeleteByPictureId(int picId);
        MVCSite.DAC.Entities.BankInfo BankInfoGetByUserID(int userId);
        MVCSite.DAC.Entities.BankInfo BankInfoGetByID(int id);
        MVCSite.DAC.Entities.BankInfo BankInfoCreateOrUpdate(MVCSite.DAC.Entities.BankInfo key);
        MVCSite.DAC.Entities.TourSchedule TourScheduleGetByID(int id);
        MVCSite.DAC.Entities.TourSchedule TourScheduleGetByTourID(int tourId);
        MVCSite.DAC.Entities.TourSchedule TourScheduleCreateOrUpdate(MVCSite.DAC.Entities.TourSchedule key);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.GuiderExcludedDates> GuiderExcludedDatesGetByUserID(int userId);
        MVCSite.DAC.Entities.GuiderExcludedDates GuiderExcludedDatesGetByID(int id);
        MVCSite.DAC.Entities.GuiderExcludedDates GuiderExcludedDatesGetByUserIDDate(int userId, string date);
        MVCSite.DAC.Entities.GuiderExcludedDates GuiderExcludedDatesCreateOrUpdate(MVCSite.DAC.Entities.GuiderExcludedDates key);
        void HandleDbEntityValidationException(System.Data.Entity.Validation.DbEntityValidationException dbEx, string function);
        void DetachAllObjectsInContext();
    }

    public partial interface IRepositoryStats
    {
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.EmailAccountToSend> EmailAccountToSendsGetTop100();
        void EmailAccountToSendsRemove(MVCSite.DAC.Entities.EmailAccountToSend account);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.QueuedEmails> QueuedEmailsGetAll();
        void QueuedEmailsRemove(MVCSite.DAC.Entities.QueuedEmails email);
        void QueuedEmailsRemoveByGuid(System.Guid id);
        void SendEmailLogCreate(MVCSite.DAC.Entities.SendEmailLog email);
        MVCSite.DAC.Entities.Visits CreateVisits(MVCSite.DAC.Entities.Visits _visits);
        MVCSite.DAC.Entities.CianQuestionLog CreateCianQuestionLog(MVCSite.DAC.Entities.CianQuestionLog _cianQuestionLog);
        void EnqueueEmail(string from, string to, string subject, string body, MVCSite.DAC.Common.SendEmailSite site, MVCSite.DAC.Common.SendEmailType type);
        void DetachAllObjectsInContext();
    }

    public partial interface IRepositoryTourists
    {
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.UserTourReview> UserTourReviewGetInPageByTourId(int tourId, int page, int pageSize);
        MVCSite.DAC.Entities.UserTourReview UserTourReviewGetByID(int id);
        MVCSite.DAC.Entities.UserTourReview UserTourReviewGetByUserIdTourId(int userId, int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourReview> UserTourReviewGetAllByTourID(int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourReview> UserTourReviewGetAllByUserID(int userId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserTourReview> UserTourReviewGetAllByTourIds(int[] tourIds);
        MVCSite.DAC.Entities.UserTourReview UserTourReviewCreateOrUpdate(MVCSite.DAC.Entities.UserTourReview key);
        MVCSite.DAC.Entities.UserWish UserWishGetByID(int id);
        MVCSite.DAC.Entities.UserWish UserWishGetByUserIdTourId(int userId, int tourId);
        void UserWishDeleteByUserIdTourId(int userId, int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserWish> UserWishGetAllByTourID(int tourId);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.UserWish> UserWishGetAllByUserID(int userId);
        MVCSite.DAC.Entities.UserWish UserWishCreateOrUpdate(MVCSite.DAC.Entities.UserWish key);
        void HandleDbEntityValidationException(System.Data.Entity.Validation.DbEntityValidationException dbEx, string function);
        void DetachAllObjectsInContext();
    }

    public partial interface IRepositoryTypes
    {
    }

    public partial interface IRepositoryUsers
    {
        bool ResetPasswordWithToken(string passwordToken, string password);
        void SetForgotPasswordToken(int userId, string passwordToken);
        MVCSite.DAC.Entities.User GetByIdOrNull(int id);
        MVCSite.DAC.Entities.User GetByEmailOrNull(string emailAddress);
        MVCSite.DAC.Entities.User GetByOpenIDOrNull(string openId, MVCSite.DAC.Entities.OpenSiteType site);
        MVCSite.DAC.Entities.User GetByMobileOrNull(string mobile);
        MVCSite.DAC.Entities.User GetByPhoneOrNull(string area, string local);
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.User> GetGlobalUsers();
        System.Linq.IQueryable<MVCSite.DAC.Entities.User> UserGetAll();
        System.Linq.IQueryable<MVCSite.DAC.Entities.User> GuideGetAll();
        System.Linq.IQueryable<MVCSite.DAC.Entities.User> TravellerGetAll();
        System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.User> UserGetAllInUserIds(int[] userIds);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.User> UserGetAllByPage(int page, int pageSize);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.User> UserGetAllByPageExceptAfter(byte lan, int userId, int page, int pageSize, System.DateTime after);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.User> UserGetAllLogonedBetweenByPage(int page, int pageSize, System.DateTime start, System.DateTime end);
        void UserRemoveCachedUser(int id);
        void UserUpdate(MVCSite.DAC.Entities.User user);
        MVCSite.DAC.Entities.User ActivateUser(int userId, string token);
        MVCSite.DAC.Entities.User ActivateUserByPassConfirmationToken(string email);
        MVCSite.DAC.Entities.User UserGetCurrentUser(int userId);
        void UserUpdateGeoInfo(MVCSite.DAC.Entities.User current);
        void UserUpdateLogonInfo(MVCSite.DAC.Entities.User current);
        void UserUpdateAvatar(MVCSite.DAC.Entities.User current);
        void UserUpdateAvatar(int id, string avatarPath);
        void UserUpdateVideo(int userId, string videoPath);
        MVCSite.DAC.Entities.User UserUpdateComfirmationToken(int userId);
        MVCSite.DAC.Entities.User UserUpdatePasswordFromOpenSite(int userId, string email, string password);
        MVCSite.DAC.Entities.UserMsg UserMsgGetByID(int id);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.UserMsg> UserMsgGetAllByToUserID(int userId, int page, int pageSize, MVCSite.DAC.Entities.UserMsgStatus status);
        MVCSite.DAC.Instrumentation.IPageable<MVCSite.DAC.Entities.UserMsg> UserMsgGetAllByUserIDs(int userId1, int userId2, int end, int page, int pageSize);
        MVCSite.DAC.Entities.UserMsg UserMsgCreateOrUpdate(MVCSite.DAC.Entities.UserMsg key);
        void UserMsgUpdateStatusById(int id, MVCSite.DAC.Entities.UserMsgStatus status);
        void UserMsgUpdateStatusByIds(string ids, MVCSite.DAC.Entities.UserMsgStatus status);
        MVCSite.DAC.Entities.User UserUpdateRole(int userId, MVCSite.Common.UserRole role);
        MVCSite.DAC.Entities.UserPromo UserPromoGetById(int userId, int promoId);
        UserPromo UserPromoCreateOrUpdate(UserPromo promo);
    }

    public partial interface ISecurity
    {
        void ValidateThatUserCanSignIn(MVCSite.DAC.Entities.User user, string password);
        void RemoveAuthenticationCookie();
        int CreateUser(string name, string password, string email);
        bool IsCurrentUserSignedIn();
        string HashPassword(string password);
        string GetCurrentUserName();
        int GetCurrentUserId();
        string GetCurrentUserInitials();
        string GetCurrentUserFullName();
        MVCSite.DAC.Instrumentation.FormsTicketDataV1 GetCurrentUserFormsTicketData();
        int GetUserIdByEmail(string email);
    }

    public partial interface IRepositoryPromos
    {
        Promo GetPromoCodeByComparing(string promoCode);
    }
}

