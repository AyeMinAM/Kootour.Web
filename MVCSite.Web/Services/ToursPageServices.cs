using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using MVCSite.Common.NameHelper;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Interfaces;

namespace MVCSite.Web.Services
{
    public class ToursPageServices
    {
        private readonly WebsiteUnityContainer _WebsiteContainer;
        private IRepositoryCities _IRepositoryCities;

        public ToursPageServices()
        {
            _WebsiteContainer = new WebsiteUnityContainer(new ConnectionStrings
            {
                kootourConnectionString = "name=kootourConnectionString",
                statConnectionString = "name=statConnectionString",

            });
            _IRepositoryCities = _WebsiteContainer.Resolve<IRepositoryCities>();
        }

        public bool IsCityName(string urlstr)
        {
            try
            {
                var paras = urlstr.Split('/');
                IEnumerable<City> allCitiesInUseCached = _IRepositoryCities.GetAllCitiesInUseCached();

                string cname = paras[1];
                //cname = cname.Split('-')[0];
                if (allCitiesInUseCached.Any(x => NameHelper.GetCityName(x.UniqueCityName).ToLower() == cname.ToLower()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }


            
        }
    }
}