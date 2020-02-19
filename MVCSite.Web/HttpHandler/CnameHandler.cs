using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Drawing;
using WMath.Facilities;
using System.Drawing.Imaging;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MVCSite.Biz;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Repositories;
using MVCSite.Common.NameHelper;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Interfaces;

namespace MVCSite.Web.HttpHandler
{
    public class CnameHandler : IHttpHandler
    {
        private readonly WebsiteUnityContainer _WebsiteContainer;
        private IRepositoryCities _IRepositoryCities;
        public CnameHandler()
        {
            _WebsiteContainer = new WebsiteUnityContainer(new ConnectionStrings
            {
                kootourConnectionString = "name=kootourConnectionString",
                statConnectionString = "name=statConnectionString",

            });
            _IRepositoryCities = _WebsiteContainer.Resolve<IRepositoryCities>();
        }
        #region IsReusable

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var paras = context.Request.Path.Split('/');
                IEnumerable<City> allCitiesInUseCached = _IRepositoryCities.GetAllCitiesInUseCached();
                
                string cname = paras[1];
                if (!allCitiesInUseCached.Any(x =>
                    NameHelper.CompareSpacedGeoNames(NameHelper.GetCityName(x.UniqueCityName), cname)))
                {

                    context.Server.TransferRequest("/Static/NotFound");
                    return;
                }

                string url = "/Tourist/Tours";
                //url += "?cname=" + cname;
                //url += "/" + cname;
                url = Path.Combine(url, cname);

                if (paras.Length > 2)
                {
                    string cat = paras[2];
                    //url += "&cat=" + cat;
                    //url += "/" + cat;
                    url = Path.Combine(url, cat);
                }

                context.Server.TransferRequest(url);
            }
            catch (Exception)
            {

                
            }


        }

    }
}
