using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Interfaces;
using MVCSite.Common;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryTypes : RepositoryBase, IRepositoryTypes
    {
        private readonly ICacheProvider _cacheProvider;
        static readonly object CategoriesCacheLock = new object();

        public RepositoryTypes(ILogger logger, EFDataContext dataContext, ICacheProvider cacheProvider)
            : base(dataContext, cacheProvider, logger)
        {
            _cacheProvider = cacheProvider;
        }

        //IEnumerable<Category> GetAllCategories()
        //{
        //    return _dataContext.Categories.ToList();
        //}

        ////cached queries
        //public IEnumerable<Category> GetAllCategoriesCached()
        //{
        //    return _cacheProvider.Get("Categories", TimeSpan.FromDays(100), CategoriesCacheLock, GetAllCategories);
        //}

    }
}
