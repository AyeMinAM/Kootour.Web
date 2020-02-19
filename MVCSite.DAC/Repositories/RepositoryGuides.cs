using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using System.Data.Objects;
using WMath.Facilities;
using System.Data.Entity.Validation;
using System.Data;
using MVCSite.DAC.Common;
using System.Data.Entity.Infrastructure;
using MVCSite.Common;
using System.Data.Entity;
using System.Linq.Expressions;
using MVCSite.DAC.Extensions;
using MVCSite.DAC.Services;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryGuides : IRepositoryGuides
    {
        static readonly object ToursCachedLock = new object();
        static readonly string cacheNameFeaturedTours = "FeaturedTours";
        protected readonly GuideDataContext _dataContext;
        protected readonly ObjectContext _objectContext;
        protected readonly ICacheProvider _cacheProvider;
        protected readonly ILogger _logger;
        public RepositoryGuides(ILogger logger, GuideDataContext dataContext, ICacheProvider cacheProvider)
        {
            _dataContext = dataContext;
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            _cacheProvider = cacheProvider;
            _logger = logger;
        }
        public UserTourBooking UserTourBookingGetByID(int id)
        {
            return _dataContext.UserTourBookings.Where(x => x.ID == id).SingleOrDefault();
        }
        public UserTourBooking UserTourBookingGetByTourIDTime(int userId, int tourId, string date, string time)
        {
            return _dataContext.UserTourBookings.Where(x => x.UserID!=userId && x.TourID == tourId && x.Calendar == date && x.Time == time).SingleOrDefault();
            //return _dataContext.UserTourBookings.Where(x => x.TourID == tourId && x.Calendar == date && x.Time == time).SingleOrDefault();
        }
        public IEnumerable<UserTourBooking> UserTourBookingGetAllByTourUserIDStartEnd(int tourUserId,DateTime start,DateTime end)
        {
            return _dataContext.UserTourBookings.Where(x => x.TourUserID == tourUserId&&x.CalendarStart>=start&&x.CalendarEnd<=end
                    &&x.Status==1);//Show paid booked tours ONLY
        }
        public IEnumerable<UserTourBooking> UserTourBookingGetAllByTourID(int tourId)
        {
            return _dataContext.UserTourBookings.Where(x => x.TourID == tourId);
        }
        public int UserTourBookingGetCountByTourID(int tourId)
        {
            return _dataContext.UserTourBookings.Where(x => x.TourID == tourId).Count();
        }
        public IEnumerable<UserTourBooking> UserTourBookingGetByTourIDDate(int tourId,string date)
        {
            return _dataContext.UserTourBookings.Where(x => x.TourID == tourId&&x.Calendar==date);
        }
        public IEnumerable<UserTourBooking> UserTourBookingGetAllByUserID(int userId)
        {
            return _dataContext.UserTourBookings.Where(x => x.UserID == userId);
        }
        public UserTourBooking UserTourBookingCreateOrUpdate(UserTourBooking key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("UserTourBookings", key);
                else
                {
                    _dataContext.UserTourBookings.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " UserTourBookingCreateOrUpdate()");
            }
            return key;
        }
        public void UserTourBookingUpdateStatus(int id, byte status)
        {
            try
            {
                var utb = this.UserTourBookingGetByID(id);
                if (utb == null)
                {
                    throw new KeyNotFoundException();
                }
                utb.Status = status;
                utb.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(utb).Property(x => x.Status).IsModified = true;
                _dataContext.Entry(utb).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserTourBookingUpdateStatus");
            }
        }
        
        public void UserTourBookingDeleteAllExpiredNoPay()
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[UserTourBooking] WHERE [Status]={0} AND [EnterTime]<'{1}';",
                (byte)UserTourBookingStatus.Initial, DateTime.UtcNow.AddMinutes(-15).ToString(TimeHelper.SQLServerDateFormat));

            _objectContext.ExecuteStoreCommand(sqlStr);
        }

        public void UserTourBookingDeleteAll()
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[UserTourBooking];");

            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public double TourGetAverageScoreForGuider(int guideid)
        {
            var allTours=_dataContext.Tours.Where(x => x.UserID == guideid);
            if(allTours == null || allTours.Count()<=0)
                return 0;
            return allTours.Average(x => x.ReviewAverageScore);
        }
        public int TourGetTotalReviewCountForGuider(int guideid)
        {
            var allTours = _dataContext.Tours.Where(x => x.UserID == guideid);
            if (allTours == null || allTours.Count() <= 0)
                return 0;
            return allTours.Sum(x => x.ReviewCount);
        }
        public List<int> TourGetAllOccupiedCityIds()
        {
            return _dataContext.Tours.Select(x=>x.TourCityID).Distinct().ToList();
        }

        public Tour TourGetByID(int id)
        {
            return _dataContext.Tours.Where(x => x.ID == id).SingleOrDefault();
        }
        public Tour TourGetByUserIDStatus(int userId,TourStatus status)
        {
            return _dataContext.Tours.Where(x => x.UserID == userId && x.Status == (byte)status).FirstOrDefault();
        }
        public IEnumerable<Tour> TourGetAllByUserID(int userId)
        {
            return _dataContext.Tours.Where(x => x.UserID == userId );
        }
        public IPageable<Tour> TourGetAllByPage(int userId,int page, int pageSize)
        {
            return _dataContext.Tours.Where(x =>x.UserID==userId && x.Status != (byte)TourStatus.Incomplete)
                .OrderByDescending(x => x.ModifyTime)
                .ToPageable(page, pageSize);
        }

        public IEnumerable<Tour> TourGetAllByIds(int [] tourIds)
        {
            return _dataContext.Tours.Where(BuildContainsExpression<Tour, int>(s => s.ID, tourIds))
                .OrderByDescending(x => x.ModifyTime);
        }
        public IQueryable<Tour> TourGetAllRichInfo()
        {
            var sqlStr = string.Format("SELECT *,c.Name FROM  [dbo].[Tour] t JOIN [dbo].[City] c on t.TourCityID = c.CityId");
            //_objectContext.CreateQuery(sqlStr);

            //_objectContext.ExecuteStoreCommand(sqlStr);
            var source = _dataContext.Tours;//.Include("City");
            var linq = from t in source
                       orderby t.ModifyTime descending
                       select t;
            return linq;
        }
        static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
            Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
        //public IPageable<Tour> TourGetFeaturedByPage(int page, int pageSize)
        //{
        //    return _dataContext.Tours.Where(x => x.IsFeatured == true)
        //        .OrderByDescending(x => x.ModifyTime)
        //        .ToPageable(page, pageSize);
        //}
        public IPageable<Tour> GetAllFeaturedToursCached()
        {
            return _cacheProvider.Get(cacheNameFeaturedTours, TimeSpan.FromDays(100), ToursCachedLock, TourGetAllFeaturedCached);
        }
        public IPageable<Tour> TourGetAllFeaturedCached()
        {
            List<Tour> tours = _dataContext.Tours.Where(x => x.Status == (byte)TourStatus.Published && x.IsFeatured == true).ToList<Tour>();

            if (tours.Count > 0)
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
                    tour.NowPrice = PriceService.CalculateVendorPromotedPrice(tour.SugRetailPrice, tourVendorPromoSubset);
                    //tour.NowPrice = schedulerPrice != null ? schedulerPrice.SugRetailPrice ?? 0 : 0;

                    var guider = userList.Where(x => x.ID == tour.UserID).FirstOrDefault();
                    var abbreviatedLastName = string.IsNullOrWhiteSpace(guider.LastName) ? "" : guider.LastName.Substring(0, 1) + ".";
                    var firstNameAbbreviatedLastName = string.Format("{0} {1}", guider.FirstName, abbreviatedLastName);

                    //tour.UserName = tourUser == null ? string.Empty : string.Format("{0} {1}", guider.FirstName, guider.LastName);
                    tour.UserName = firstNameAbbreviatedLastName;
                });
            }
            return tours.OrderByDescending(x => x.ModifyTime).ToPageable(1, 100);
        }
        public void TourUpdateFeaturedTour(int id, bool status)
        {
            try
            {
                var tour = this.TourGetByID(id);
                if (tour == null)
                {
                    throw new KeyNotFoundException();
                }
                tour.IsFeatured = status;
                tour.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(tour).Property(x => x.Status).IsModified = true;
                _dataContext.Entry(tour).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "TourUpdateStatus");
            }
        }
        public IPageable<Tour> TourGetAllInCityByPage(SearchCriteria criteria, int page, int pageSize)
        {
            var tourIds=_dataContext.TourSchedules

                //Remove Date filtering from criteria
                //Two lines related to Date filtering
                //One in TouristController; The other in RepositoryGuides
                //.Where(x=>x.BeginDate<=criteria.OnDate&&x.EndDate>=criteria.OnDate)

                .Select(x => x.TourID).ToArray();

            //IQueryable<Tour> tours = _dataContext.Tours
            //    .Where(x => x.TourCityID == criteria.Cityid && x.Status == (byte)TourStatus.Published).Where(BuildContainsExpression<Tour, int>(s => s.ID, tourIds));
            IQueryable<Tour> tours = _dataContext.Tours
                .Where(x => x.TourCityID == criteria.Cityid)
                .Where(x => x.Status == (byte)TourStatus.Published)
                .Where(BuildContainsExpression<Tour, int>(s => s.ID, tourIds));

            if (!criteria.IsAllLanguage)
            {
                if (criteria.IsEnglish)
                    tours = tours.Where(x => x.IsEnglish);
                if (criteria.IsChineseMandarian)
                    tours = tours.Where(x => x.IsChineseMandarian);
                if (criteria.IsChineseCantonese)
                    tours = tours.Where(x => x.IsChineseCantonese);
                if (criteria.IsFrench)
                    tours=tours.Where(x => x.IsFrench);
                if (criteria.IsSpanish)
                    tours = tours.Where(x => x.IsSpanish);
                if (criteria.IsGerman)
                    tours = tours.Where(x => x.IsGerman);
                if (criteria.IsPortuguese)
                    tours = tours.Where(x => x.IsPortuguese);
                if (criteria.IsItalian)
                    tours = tours.Where(x => x.IsItalian);
                if (criteria.IsRussian)
                    tours = tours.Where(x => x.IsRussian);
                if (criteria.IsKorean)
                    tours = tours.Where(x => x.IsKorean);
                if (criteria.IsJapanese)
                    tours = tours.Where(x => x.IsJapanese);
                if (criteria.IsNorwegian)
                    tours = tours.Where(x => x.IsNorwegian);
                if (criteria.IsSwedish)
                    tours = tours.Where(x => x.IsSwedish);
                if (criteria.IsDanish)
                    tours = tours.Where(x => x.IsDanish);
            }
            if (!criteria.IsAllCategory)
            {
                if (criteria.IsHistorical)
                    tours = tours.Where(x => x.IsHistorical);
                if (criteria.IsAdventure)
                    tours = tours.Where(x => x.IsAdventure);
                if (criteria.IsLeisureSports)
                    tours = tours.Where(x => x.IsLeisureSports);
                if (criteria.IsCultureArts)
                    tours = tours.Where(x => x.IsCultureArts);
                if (criteria.IsNatureRural)
                    tours = tours.Where(x => x.IsNatureRural);
                if (criteria.IsFestivalEvents)
                    tours = tours.Where(x => x.IsFestivalEvents);
                if (criteria.IsNightlifeParty)
                    tours = tours.Where(x => x.IsNightlifeParty);
                if (criteria.IsFoodDrink)
                    tours = tours.Where(x => x.IsFoodDrink);
                if (criteria.IsShoppingMarket)
                    tours = tours.Where(x => x.IsShoppingMarket);
                if (criteria.IsTransportation)
                    tours = tours.Where(x => x.IsTransportation);
                if (criteria.IsBusinessInterpretation)
                    tours = tours.Where(x => x.IsBusinessInterpretation);
                if (criteria.IsPhotography)
                    tours = tours.Where(x => x.IsPhotography);
            }
            //return tours.OrderByDescending(x => x.ModifyTime).ToPageable(page, pageSize);
            //return tours.OrderBy(x => x.Name).ThenByDescending(x => x.ModifyTime).ToPageable(page, pageSize);
            return tours.OrderByDescending(x => x.Name).ToPageable(page, pageSize);
        }

        public IPageable<Tour> TourGetAllBySearchModeByPage(SearchCriteria criteria, int page, int pageSize, byte mode)
        {
            var tourIds = _dataContext.TourSchedules
                .Select(x => x.TourID).ToArray();
            //Remove Date filtering from criteria
            //Two lines related to Date filtering
            //One in TouristController; The other in RepositoryGuides
            //.Where(x=>x.BeginDate<=criteria.OnDate&&x.EndDate>=criteria.OnDate)

            IQueryable<Tour> tours = _dataContext.Tours;
            switch (mode)
            {
                case 0:
                    tours = tours.Where(x => x.TourCityID == criteria.Cityid);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            tours = tours.Where(x => x.Status == (byte)TourStatus.Published)
                         .Where(BuildContainsExpression<Tour, int>(s => s.ID, tourIds));

            if (!criteria.IsAllLanguage)
            {
                if (criteria.IsEnglish)
                    tours = tours.Where(x => x.IsEnglish);
                if (criteria.IsChineseMandarian)
                    tours = tours.Where(x => x.IsChineseMandarian);
                if (criteria.IsChineseCantonese)
                    tours = tours.Where(x => x.IsChineseCantonese);
                if (criteria.IsFrench)
                    tours = tours.Where(x => x.IsFrench);
                if (criteria.IsSpanish)
                    tours = tours.Where(x => x.IsSpanish);
                if (criteria.IsGerman)
                    tours = tours.Where(x => x.IsGerman);
                if (criteria.IsPortuguese)
                    tours = tours.Where(x => x.IsPortuguese);
                if (criteria.IsItalian)
                    tours = tours.Where(x => x.IsItalian);
                if (criteria.IsRussian)
                    tours = tours.Where(x => x.IsRussian);
                if (criteria.IsKorean)
                    tours = tours.Where(x => x.IsKorean);
                if (criteria.IsJapanese)
                    tours = tours.Where(x => x.IsJapanese);
                if (criteria.IsNorwegian)
                    tours = tours.Where(x => x.IsNorwegian);
                if (criteria.IsSwedish)
                    tours = tours.Where(x => x.IsSwedish);
                if (criteria.IsDanish)
                    tours = tours.Where(x => x.IsDanish);
            }
            if (!criteria.IsAllCategory)
            {
                if (criteria.IsHistorical)
                    tours = tours.Where(x => x.IsHistorical);
                if (criteria.IsAdventure)
                    tours = tours.Where(x => x.IsAdventure);
                if (criteria.IsLeisureSports)
                    tours = tours.Where(x => x.IsLeisureSports);
                if (criteria.IsCultureArts)
                    tours = tours.Where(x => x.IsCultureArts);
                if (criteria.IsNatureRural)
                    tours = tours.Where(x => x.IsNatureRural);
                if (criteria.IsFestivalEvents)
                    tours = tours.Where(x => x.IsFestivalEvents);
                if (criteria.IsNightlifeParty)
                    tours = tours.Where(x => x.IsNightlifeParty);
                if (criteria.IsFoodDrink)
                    tours = tours.Where(x => x.IsFoodDrink);
                if (criteria.IsShoppingMarket)
                    tours = tours.Where(x => x.IsShoppingMarket);
                if (criteria.IsTransportation)
                    tours = tours.Where(x => x.IsTransportation);
                if (criteria.IsBusinessInterpretation)
                    tours = tours.Where(x => x.IsBusinessInterpretation);
                if (criteria.IsPhotography)
                    tours = tours.Where(x => x.IsPhotography);
            }
               
            
            switch (criteria.Duration)
            {
                case 1://half day
                    {
                        tours = tours.Where(x => x.Duration <= 5    & x.DurationTimeType == (byte)TourTimeType.Hours).
                          Union(tours.Where( x=> x.Duration <= 5*60 & x.DurationTimeType == (byte)TourTimeType.Minutes));
                        break;
                    }
                case 2://1day
                    {                                                                       
                        tours = tours.Where(x => x.Duration > 5   & x.Duration <= 24 & x.DurationTimeType == (byte)TourTimeType.Hours). 
                          Union(tours.Where(x => x.Duration> 5*60 & x.Duration <= 24*60 & x.DurationTimeType == (byte)TourTimeType.Minutes));
                        break;
                    }
                case 3://multi day
                    {
                        tours = tours.Where(x => x.Duration > 1     & x.DurationTimeType == (byte)TourTimeType.Days).
                          Union(tours.Where(x => x.Duration  > 24   & x.DurationTimeType == (byte)TourTimeType.Hours).
                          Union(tours.Where(x => x.Duration > 24*60 & x.DurationTimeType == (byte)TourTimeType.Minutes)));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }



          

            //return tours.OrderByDescending(x => x.Name).ToPageable(page, pageSize);
            //return tours.OrderBy(x => x.Name).ThenByDescending(x => x.ModifyTime).ToPageable(page, pageSize);
            return tours.OrderByDescending(x => x.ModifyTime).ToPageable(page, pageSize);
        }
        public Tour TourCreateOrUpdate(Tour key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("Tours", key);
                else
                {
                    _dataContext.Tours.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourCreateOrUpdate()");
            }
            return key;
        }

        

        public void TourDeleteByID(int tourId)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourPicture] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[TourSchedule] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[TourInclusion] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[TourExclusion] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[TourExtra] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[UserWish] WHERE [TourID]={0};", tourId);
            sqlStr += string.Format("DELETE FROM [dbo].[UserTourReview] WHERE [TourID]={0};", tourId);            
            sqlStr += string.Format("DELETE FROM [dbo].[UserTourBooking] WHERE [TourID]={0};", tourId);            
            sqlStr += string.Format("DELETE FROM [dbo].[Tour] WHERE [ID]={0};", tourId);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public Tour TourCloneByID(int tourId)
        {
            var tour=TourGetByID(tourId);
            if (tour == null)
                return null;
            var newTour = new Tour(tour);
            newTour.ID = 0;
            newTour.Name += " Cloned@"+DateTime.Now.ToShortTimeString();
            newTour.IsFeatured = false;
            TourCreateOrUpdate(newTour);
            var tourSchudule = TourScheduleGetByTourID(tourId);
            if (tourSchudule != null)
            {
                var newTourSchudule = new TourSchedule(tourSchudule);
                newTourSchudule.ID = 0;
                newTourSchudule.TourID = newTour.ID;
                TourScheduleCreateOrUpdate(newTourSchudule);
            }

            var tourExclusions = TourExclusionGetAllByTourID(tourId).ToList();
            tourExclusions.ForEach(tourExclusion => {
                var newTourExclusion = new TourExclusion(tourExclusion);
                newTourExclusion.ID = 0;
                newTourExclusion.TourID = newTour.ID;
                TourExclusionCreateOrUpdate(newTourExclusion);
            });
            var tourInclusions = TourInclusionGetAllByTourID(tourId).ToList();
            tourInclusions.ForEach(tourInclusion =>
            {
                var newTourInclusion = new TourInclusion(tourInclusion);
                newTourInclusion.ID = 0;
                newTourInclusion.TourID = newTour.ID;
                TourInclusionCreateOrUpdate(newTourInclusion);
            });
            var tourPictures = TourPictureGetAllByTourID(tourId).ToList();
            tourPictures.ForEach(tourPicture =>
            {
                var newTourPicture = new TourPicture(tourPicture);
                newTourPicture.ID = 0;
                newTourPicture.TourID = newTour.ID;
                TourPictureCreateOrUpdate(newTourPicture);
            });
            var tourExtras = TourExtraGetAllByTourID(tourId).ToList();
            tourExtras.ForEach(tourExtra =>
            {
                var newTourExtra = new TourExtra(tourExtra);
                newTourExtra.ID = 0;
                newTourExtra.TourID = newTour.ID;
                TourExtraCreateOrUpdate(newTourExtra);
            });
            return newTour;
        }
        public void TourUpdateStatus(int id, byte status)
        {
            try
            {
                var tour = this.TourGetByID(id);
                if (tour == null)
                {
                    throw new KeyNotFoundException();
                }
                tour.Status = status;
                tour.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(tour).Property(x => x.Status).IsModified = true;
                _dataContext.Entry(tour).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "TourUpdateStatus");
            }
        }

        public void TourUpdateReviewInfoByTourId(int tourId)
        {
            try
            {
                var tour = this.TourGetByID(tourId);
                if (tour == null)
                {
                    throw new KeyNotFoundException();
                }
                var reviewCount = _dataContext.UserTourReviews.Where(x => x.TourID == tourId).Count();
                var totalScore = _dataContext.UserTourReviews.Where(x => x.TourID == tourId).Sum(x=>x.AverageScore);
                tour.ReviewCount = reviewCount;
                tour.ReviewAverageScore = totalScore / reviewCount;
                tour.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(tour).Property(x => x.ReviewCount).IsModified = true;
                _dataContext.Entry(tour).Property(x => x.ReviewAverageScore).IsModified = true;
                _dataContext.Entry(tour).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "TourUpdateStatus");
            }
        }
        public TourExtra TourExtraGetByID(int id)
        {
            return _dataContext.TourExtras.Where(x => x.ID == id).SingleOrDefault();
        }
        public TourExtra TourExtraGetByTourIDSortNo(int tourId, byte sortNo)
        {
            return _dataContext.TourExtras.Where(x => x.TourID == tourId && x.SortNo == sortNo).SingleOrDefault();
        }
        public IEnumerable<TourExtra> TourExtraGetAllByTourID(int tourId)
        {
            return _dataContext.TourExtras.Where(x => x.TourID == tourId);
        }
        public IEnumerable<TourExtra> TourExtraGetAllByIds(int[] tourExtraIds)
        {
            return _dataContext.TourExtras.Where(BuildContainsExpression<TourExtra, int>(s => s.ID, tourExtraIds))
                .OrderByDescending(x => x.ModifyTime);
        }
        public TourExtra TourExtraCreateOrUpdate(TourExtra key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourExtras", key);
                else
                {
                    _dataContext.TourExtras.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourExtraCreateOrUpdate()");
            }
            return key;
        }

        public TourInclusion TourInclusionGetByID(int id)
        {
            return _dataContext.TourInclusions.Where(x => x.ID == id).SingleOrDefault();
        }
        public TourInclusion TourInclusionGetByTourIDSortNo(int tourId,byte sortNo)
        {
            return _dataContext.TourInclusions.Where(x => x.TourID == tourId&&x.SortNo==sortNo).SingleOrDefault();
        }
        public IEnumerable<TourInclusion> TourInclusionGetAllByTourID(int tourId)
        {
            return _dataContext.TourInclusions.Where(x => x.TourID == tourId);
        }
        public TourInclusion TourInclusionCreateOrUpdate(TourInclusion key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourInclusions", key);
                else
                {
                     _dataContext.TourInclusions.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourInclusionCreateOrUpdate()");
            }
            return key;
        }

        public TourExclusion TourExclusionGetByID(int id)
        {
            return _dataContext.TourExclusions.Where(x => x.ID == id).SingleOrDefault();
        }
        public TourExclusion TourExclusionGetByTourIDSortNo(int tourId, byte sortNo)
        {
            return _dataContext.TourExclusions.Where(x => x.TourID == tourId && x.SortNo == sortNo).SingleOrDefault();
        }
        public IEnumerable<TourExclusion> TourExclusionGetAllByTourID(int tourId)
        {
            return _dataContext.TourExclusions.Where(x => x.TourID == tourId);
        }
        public TourExclusion TourExclusionCreateOrUpdate(TourExclusion key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourExclusions", key);
                else
                {
                    _dataContext.TourExclusions.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourExclusionCreateOrUpdate()");
            }
            return key;
        }
        public IEnumerable<TourPriceBreakdown> TourPriceBreakdownGetAllByTourID(int tourId)
        {
            return _dataContext.TourPriceBreakdowns.Where(x => x.TourID == tourId);
        }
        public TourPriceBreakdown TourPriceBreakdownGetByTourIDSortNo(int tourId, byte sortNo)
        {
            return _dataContext.TourPriceBreakdowns.Where(x => x.TourID == tourId && x.SortNo == sortNo).SingleOrDefault();
        }
        public TourPriceBreakdown TourPriceBreakdownCreateOrUpdate(TourPriceBreakdown key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourPriceBreakdown", key);
                else
                {
                    _dataContext.TourPriceBreakdowns.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourPriceBreakdownCreateOrUpdate()");
            }
            return key;
        }
        public void TourInclusionDeleteOld(int tourId, byte sortNo)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourInclusion] WHERE [TourID]={0} and [SortNo]>={1};", tourId, sortNo);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public void TourExclusionDeleteOld(int tourId, byte sortNo)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourExclusion] WHERE [TourID]={0} and [SortNo]>={1};", tourId, sortNo);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public void TourPriceBreakdownDeleteOld(int tourId, byte sortNo)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourPriceBreakdown] WHERE [TourID]={0} and [SortNo]>={1};", tourId, sortNo);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public IEnumerable<TourVendorPromo> TourVendorPromoGetAllByTourID(int tourId)
        {
            return _dataContext.TourVendorPromoes.Where(x => x.TourID == tourId);
        }
        public TourVendorPromo TourVendorPromoGetByTourIDSortNo(int tourId, byte sortNo)
        {
            return _dataContext.TourVendorPromoes.Where(x => x.TourID == tourId && x.SortNo == sortNo).SingleOrDefault();
        }
        public TourVendorPromo TourVendorPromoCreateOrUpdate(TourVendorPromo key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourVendorPromo", key);
                else
                {
                    _dataContext.TourVendorPromoes.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourVendorPromoCreateOrUpdate()");
            }
            return key;
        }
        public void TourVendorPromoDeleteOld(int tourId, byte sortNo)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourVendorPromo] WHERE [TourID]={0} and [SortNo]>={1};", tourId, sortNo);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }

        public TourPicture TourPictureGetByID(int id)
        {
            return _dataContext.TourPictures.Where(x => x.ID == id).SingleOrDefault();
        }
        public IEnumerable<TourPicture> TourPictureGetAllByTourID(int tourId)
        {
            return _dataContext.TourPictures.Where(x => x.TourID == tourId);
        }
        public int TourPictureGetTotalCountOfTourId(int tourId)
        {
            return _dataContext.TourPictures.Where(x => x.TourID == tourId).Count();
        }
        public TourPicture TourPictureCreateOrUpdate(TourPicture key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourPictures", key);
                else
                {
                    _dataContext.TourPictures.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourPictureCreateOrUpdate()");
            }
            return key;
        }
        public void TourPictureDeleteByPictureId(int picId)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[TourPicture] WHERE [ID]={0};", picId);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public BankInfo BankInfoGetByUserID(int userId)
        {
            return _dataContext.BankInfoes.Where(x => x.UserID == userId).SingleOrDefault();
        }
        public BankInfo BankInfoGetByID(int id)
        {
            return _dataContext.BankInfoes.Where(x => x.ID == id).SingleOrDefault();
        }
        public BankInfo BankInfoCreateOrUpdate(BankInfo key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("BankInfoes", key);
                else
                {
                    _dataContext.BankInfoes.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " BankInfoCreateOrUpdate()");
            }
            return key;
        }


        public TourSchedule TourScheduleGetByID(int id)
        {
            return _dataContext.TourSchedules.Where(x => x.ID == id).SingleOrDefault();
        }
        public TourSchedule TourScheduleGetByTourID(int tourId)
        {
            return _dataContext.TourSchedules.Where(x => x.TourID == tourId).SingleOrDefault();
        }
        public TourSchedule TourScheduleCreateOrUpdate(TourSchedule key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("TourSchedules", key);
                else
                {
                    var oldOne = TourScheduleGetByTourID(key.TourID);
                    if (oldOne != null)
                    {//There are 2 dupliate terms in the same feed, we only take the first one.
                        return oldOne;
                    }
                    _dataContext.TourSchedules.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " TourScheduleCreateOrUpdate()");
            }
            return key;
        }

        public IEnumerable<GuiderExcludedDates> GuiderExcludedDatesGetByUserID(int userId)
        {
            return _dataContext.GuiderExcludedDates.Where(x => x.UserID == userId&&x.Status == 1);
        }
        public GuiderExcludedDates GuiderExcludedDatesGetByID(int id)
        {
            return _dataContext.GuiderExcludedDates.Where(x => x.ID == id).SingleOrDefault();
        }
        public GuiderExcludedDates GuiderExcludedDatesGetByUserIDDate(int userId,string date)
        {
            return _dataContext.GuiderExcludedDates.Where(x =>x.UserID == userId && x.Date == date).SingleOrDefault();
        }
        public GuiderExcludedDates GuiderExcludedDatesCreateOrUpdate(GuiderExcludedDates key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("GuiderExcludedDates", key);
                else
                {
                    var oldOne = GuiderExcludedDatesGetByUserIDDate(key.UserID, key.Date);
                    if (oldOne != null)
                    {//There are 2 dupliate terms in the same feed, we only take the first one.
                        return oldOne;
                    }
                    _dataContext.GuiderExcludedDates.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " GuiderExcludedDatesCreateOrUpdate()");
            }
            return key;
        }




        public void HandleDbEntityValidationException(DbEntityValidationException dbEx, string function)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    var errorMsg = string.Format("ValidationError--Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    sb.Append(errorMsg);
                    sb.AppendLine();
                }
            }
            _logger.LogError(string.Format("{0}() GOT exception message:{1}.",
                function, sb.ToString()));
            return;
        }
        protected void EnsureAttachedAndModified<T>(string entitySetName, T entity) where T : class
        {
            if (_dataContext.Entry(entity).State == EntityState.Detached)
            {
                _objectContext.AttachTo(entitySetName, entity);
            }
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public void DetachAllObjectsInContext()
        {
            var objectStateEntries = _objectContext
                            .ObjectStateManager
                            .GetObjectStateEntries(EntityState.Added | EntityState.Deleted |
                            EntityState.Modified | EntityState.Unchanged);
            foreach (var objectStateEntry in objectStateEntries)
            {
                _objectContext.Detach(objectStateEntry.Entity);
            }
            //_objectContext.SaveChanges();
            _objectContext.Dispose();

        }
        void MarkAsDeleted<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
                _dataContext.Entry(entity).State = EntityState.Deleted;
        }
    }
}