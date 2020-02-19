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

namespace MVCSite.DAC.Repositories
{
    public class RepositoryTourists : IRepositoryTourists
    {
        protected readonly GuideDataContext _dataContext;
        protected readonly ObjectContext _objectContext;
        protected readonly ICacheProvider _cacheProvider;
        protected readonly ILogger _logger;
        public RepositoryTourists(ILogger logger, GuideDataContext dataContext, ICacheProvider cacheProvider)
        {
            _dataContext = dataContext;
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            _cacheProvider = cacheProvider;
            _logger = logger;
        }
        public IPageable<UserTourReview> UserTourReviewGetInPageByTourId(int tourId, int page, int pageSize)
        {
            return _dataContext.UserTourReviews.Where(x => x.TourID == tourId)
                .OrderByDescending(x => x.EnterTime).ToPageable(page, pageSize);
        }
        public UserTourReview UserTourReviewGetByID(int id)
        {
            return _dataContext.UserTourReviews.Where(x => x.ID == id).SingleOrDefault();
        }
        public UserTourReview UserTourReviewGetByUserIdTourId(int userId, int tourId)
        {
            return _dataContext.UserTourReviews.Where(x => x.UserID == userId && x.TourID == tourId).SingleOrDefault();
        }
        public IEnumerable<UserTourReview> UserTourReviewGetAllByTourID(int tourId)
        {
            return _dataContext.UserTourReviews.Where(x => x.TourID == tourId);
        }
        public IEnumerable<UserTourReview> UserTourReviewGetAllByUserID(int userId)
        {
            return _dataContext.UserTourReviews.Where(x => x.UserID == userId);
        }
        public IEnumerable<UserTourReview> UserTourReviewGetAllByTourIds(int[] tourIds)
        {
            return _dataContext.UserTourReviews.Where(BuildContainsExpression<UserTourReview, int>(s => s.TourID, tourIds))                .OrderByDescending(x => x.ModifyTime);
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

        public UserTourReview UserTourReviewCreateOrUpdate(UserTourReview key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("UserTourReviews", key);
                else
                {
                    _dataContext.UserTourReviews.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " UserTourReviewCreateOrUpdate()");
            }
            return key;
        }

        public UserWish UserWishGetByID(int id)
        {
            return _dataContext.UserWishes.Where(x => x.ID == id).SingleOrDefault();
        }
        public UserWish UserWishGetByUserIdTourId(int userId,int tourId)
        {
            return _dataContext.UserWishes.Where(x => x.UserID == userId && x.TourID == tourId).SingleOrDefault();
        }
        public void UserWishDeleteByUserIdTourId(int userId, int tourId)
        {
            var sqlStr = string.Format("DELETE FROM [dbo].[UserWish] WHERE  [UserID]={0} AND [TourID]={1};", userId, tourId);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public IEnumerable<UserWish> UserWishGetAllByTourID(int tourId)
        {
            return _dataContext.UserWishes.Where(x => x.TourID == tourId);
        }
        public IEnumerable<UserWish> UserWishGetAllByUserID(int userId)
        {
            return _dataContext.UserWishes.Where(x => x.UserID == userId);
        }
        public UserWish UserWishCreateOrUpdate(UserWish key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("UserWishes", key);
                else
                {
                    _dataContext.UserWishes.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " UserWishCreateOrUpdate()");
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