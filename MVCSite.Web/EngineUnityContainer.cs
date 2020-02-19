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
using MVCSite.Biz;

namespace MVCSite.DAC.Instrumentation
{
    public class EngineUnityContainer : BizUnityContainer
    {
        public EngineUnityContainer(ConnectionStrings cons) :
            base(cons)
        {
            //Instrumentation classes

            //this.RegisterType<Tracker>(new SingletonLifetimeManager());

            //Services layer

            //this.RegisterType<ISecurity, Security>(new SingletonLifetimeManager());

            ////Data layer

        }


    }
}
