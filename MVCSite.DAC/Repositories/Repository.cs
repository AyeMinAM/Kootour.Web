using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Interfaces;
using WMath.Facilities;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
namespace MVCSite.DAC.Repositories
{
    public class Repository : RepositoryBase, IRepository
    {
        public Repository(ILogger logger, EFDataContext dataContext, ICacheProvider cacheProvider) : base(dataContext, cacheProvider, logger) { }



    }
}