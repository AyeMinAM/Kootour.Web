using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using WMath.Facilities;
using MVCSite.DAC.Instrumentation.Membership;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Services;
using SingletonLifetimeManager = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;
using MVCSite.DAC.Repositories;
using System.Security;
using MVCSite.DAC.Common;
using MVCSite.Common;
namespace MVCSite.DAC.Instrumentation
{
    public class ConnectionStrings
    {
        public string kootourConnectionString;
        public string statConnectionString;        

    }
    public class DACUnityContainer : UnityContainer
    {
        public DACUnityContainer(ConnectionStrings  cons)
        {

            this.RegisterType<ISecurity, Security>(new SingletonLifetimeManager());
            this.RegisterType<ILogger, Logger>(new SingletonLifetimeManager());
            this.RegisterType<ISiteConfiguration, SiteConfiguration>(new SingletonLifetimeManager());

            if (!string.IsNullOrEmpty(cons.kootourConnectionString))
            {
                this.RegisterType<EFDataContext>(new PerRequestLifetimeManager(),
                                                new InjectionFactory(c => new EFDataContext(cons.kootourConnectionString, null)));
                //per resolve lifetime strategy is required for some create methods inside membership provider
                this.RegisterType<EFDataContext>("perResolveRepository", new PerResolveLifetimeManager(),
                                              new InjectionFactory(c => new EFDataContext(cons.kootourConnectionString, null)));
                this.RegisterType<GuideDataContext>(new PerRequestLifetimeManager(),
                                                new InjectionFactory(c => new GuideDataContext(cons.kootourConnectionString, null)));
            }

            if (!string.IsNullOrEmpty(cons.statConnectionString))
            {
                this.RegisterType<StatDataContext>(new PerRequestLifetimeManager(),
                                                new InjectionFactory(c => new StatDataContext(cons.statConnectionString, null)));
            }

            this.RegisterType<ICacheProvider, HttpCacheProvider>(new SingletonLifetimeManager());
            //Data layer
            this.RegisterType<IRepository, Repository>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryUsers, RepositoryUsers>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryStats, RepositoryStats>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryGuides, RepositoryGuides>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryCities, RepositoryCities>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryTypes, RepositoryTypes>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryTourists, RepositoryTourists>(new PerRequestLifetimeManager());
            this.RegisterType<IRepositoryPromos, RepositoryPromos>(new PerRequestLifetimeManager());
        }

    }
}
