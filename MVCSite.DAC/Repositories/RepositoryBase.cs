using System;
using System.Linq;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Collections.Generic;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Repositories;
using System.Text;
using System.Data.Entity.Validation;
using MVCSite.Common;

namespace MVCSite.DAC.Interfaces
{
    public partial interface IRepository : IRepositoryBase { }
}

namespace MVCSite.DAC.Repositories
{
    public interface IRepositoryBase
    {
        int SaveChanges();
    }

    public class RepositoryBase : IRepositoryBase
    {
        protected readonly ILogger _logger;
        protected readonly EFDataContext _dataContext;
        protected readonly ObjectContext _objectContext;
        protected readonly ICacheProvider _cacheProvider;
        protected static readonly object _locationCacheLock = new object();
        public RepositoryBase(EFDataContext dataContext, ICacheProvider cacheProvider, ILogger logger)
        {
            _dataContext = dataContext;
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            _cacheProvider = cacheProvider;
            _logger = logger;
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }
        protected void EnsureAttachedAndModified<T>(string entitySetName, T entity) where T : class
        {
            if (_dataContext.Entry(entity).State == EntityState.Detached)
            {
                _objectContext.AttachTo(entitySetName, entity);
            }
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        protected void MarkAsDeleted<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
                _dataContext.Entry(entity).State = EntityState.Deleted;
        }

        public void HandleDbEntityValidationException(DbEntityValidationException dbEx,string function)
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
    }
}