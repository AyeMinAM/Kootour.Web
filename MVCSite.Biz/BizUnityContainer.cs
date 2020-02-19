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
using MVCSite.DAC.Instrumentation;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Common;

namespace MVCSite.Biz
{
    public class BizUnityContainer : DACUnityContainer
    {
        public BizUnityContainer(ConnectionStrings cons)
            : base(cons)
        {

            this.RegisterType<Tracker>(new SingletonLifetimeManager());
            this.RegisterType<IEmailer, Emailer>(new SingletonLifetimeManager());
            this.RegisterType<EmailGenerator>(new SingletonLifetimeManager());
            this.RegisterType<IWebApplicationContext, WebApplicationContext>(new SingletonLifetimeManager());


            this.RegisterType<PageFlow>(new SingletonLifetimeManager());
            this.RegisterType<IPublicCommands, PublicCommands>(new PerRequestLifetimeManager());

            RegisterTypeSecure<IUserQueries, UserQueries>(() => new PerRequestLifetimeManager(), x => x.IsCurrentUserSignedIn());
            RegisterTypeSecure<IUserCommands, UserCommands>(() => new PerRequestLifetimeManager(), x => x.IsCurrentUserSignedIn());

            RegisterTypeSecure<ICommunityCommands, CommunityCommands>(() => new PerRequestLifetimeManager(), x => x.IsCurrentUserSignedIn());

        }

        void RegisterTypeSecure<TInterface, TImplementation>(Func<PerRequestLifetimeManager> lifetimeFactory, Func<ISecurity, bool> checkFunc) where TImplementation : TInterface
        {
            this.RegisterType<TInterface, TImplementation>("noSecurityChecks", lifetimeFactory());
            this.RegisterType<TInterface, TImplementation>(lifetimeFactory(),
                new InjectionFactory(c =>
                {
                    var security = c.Resolve<ISecurity>();
                    if (!checkFunc(security))
                        throw new SecurityException("Security rules violation detected");
                    return c.Resolve<TInterface>("noSecurityChecks");
                }));
        }
    }
}
