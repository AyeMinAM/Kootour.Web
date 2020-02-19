using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using System.Data.Objects;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Interfaces;
using MVCSite.Common;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryPromos : IRepositoryPromos
    {
        protected readonly GuideDataContext _dataContext;
        protected readonly ObjectContext _objectContext;
        protected readonly ICacheProvider _cacheProvider;
        protected readonly ILogger _logger;
        public RepositoryPromos(ILogger logger, GuideDataContext dataContext, ICacheProvider cacheProvider)
        {
            _dataContext = dataContext;
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            _cacheProvider = cacheProvider;
            _logger = logger;
        }

        public Promo GetPromoCodeByComparing(string code)
        {
            return _dataContext.Promoes.Where(x => x.Code.ToLower() == code.ToLower()).FirstOrDefault();
        }
        
    }
}
