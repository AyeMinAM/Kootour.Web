using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using WMath.Facilities;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Services;
using SingletonLifetimeManager = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;

namespace MVCSite.Web
{
    public class WebsiteUnityContainer : EngineUnityContainer
    {
        public WebsiteUnityContainer(ConnectionStrings cons) :
            base(cons) { }
    }
}