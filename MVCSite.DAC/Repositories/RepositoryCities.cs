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
using System.Web.Mvc;
using MVCSite.DAC.Extensions;
using DevTrends.MvcDonutCaching;
using System.Web;
using MVCSite.Common.NameHelper;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryCities : RepositoryBase, IRepositoryCities
    {
        private readonly ICacheProvider _cacheProvider;
        static readonly object CitiesCacheLock = new object();
        static readonly object CountriesCacheLock = new object();
        static readonly object CityLettersCacheLock = new object();
        static readonly object CitiesSelectListCacheLock = new object();
        static readonly object PhoneCodesSelectListCacheLock = new object();
        static readonly string cacheNameCityInUse = "CityInUse";

        public RepositoryCities(ILogger logger, EFDataContext dataContext, ICacheProvider cacheProvider)
            : base(dataContext, cacheProvider, logger)
        {
            _cacheProvider = cacheProvider;
        }
        //not cached queries
        IEnumerable<City> GetAllCitiesInUse()
        {
            return _dataContext.Cities.Where(x => x.IsInUse == true).OrderBy(x=>x.Name).ToList();
        }
        IEnumerable<InternatinalPhoneCode> GetAllInternatinalPhoneCode()
        {
            return _dataContext.InternatinalPhoneCodes.Where(x => x.Country != "DISCONTINUED").ToList();
        }
        IEnumerable<Country> GetAllCountries()
        {
            return _dataContext.Countries.ToList();
        }
        public City GetCityByUniqueNameOrNullInDB(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return _dataContext.Cities.Where(x => x.UniqueCityName.ToLower() == name.ToLower() && x.IsInUse==true).FirstOrDefault();
        }
        public City GetCityByUniqueCityNameOrNullInDB(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return _dataContext.Cities.Where(x => x.UniqueCityName.ToLower().Contains(name.ToLower()) && x.IsInUse == true).FirstOrDefault();
        }
        public City GetCityByUniqueCityNameOrNullInDBViaDashedNames(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return _dataContext.Cities.Where(x => x.UniqueCityName.ToLower().Replace(" ","-").Contains(name.ToLower()) && x.IsInUse == true).FirstOrDefault();
        }

        public City GetCityByNameInUrlOrNullInDB(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return _dataContext.Cities.Where(x => x.UniqueCityNameInURL.ToLower() == name.ToLower() && x.IsInUse==true).FirstOrDefault();
        }
        public City GetCityByIdInDB(int cityId)
        {
            return _dataContext.Cities.Where(x => x.CityId == cityId).SingleOrDefault();
        }

        public List<City> GetCityListByIdList(IEnumerable<int> cityList)
        {
            if (cityList==null  || !cityList.Any())
            {
                return null;
            }
            List<City> returnList = new List<City>();
            foreach (var id in cityList)
            {
                var result = from c1 in _dataContext.Cities
                             where c1.CityId == id
                             select c1;
                returnList.Add(result.ToList()[0]);
            }

            return returnList;
        }

        public Country CountryCreateOrGet(string country)
        {
            try
            {
                var oldOne = _dataContext.Countries.Where(x => x.description.ToLower() == country.ToLower()).SingleOrDefault();
                if (oldOne != null)
                    return oldOne;
                Country key = new Country() { 
                    description=country,
                    isbanned=false,
                    abbreviation = country.Substring(0,2)
                };
                _dataContext.Countries.Add(key);
                _dataContext.SaveChanges();
                return key;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " CountryCreate()");
            }
            return null;
        }
        public City CityCreateOrGet(string name, string uniqueName, string nameInUrl, int countrId, int regionId)
        {
            try
            {
                var oldOne = _dataContext.Cities.Where(x => x.UniqueCityNameInURL.ToLower() == nameInUrl.ToLower()).SingleOrDefault();
                if (oldOne != null)
                    return oldOne;
                City key = new City()
                {
                    CountryID=countrId,
                    RegionID=regionId,
                    Name=name,
                    Latitude=string.Empty,
                    Longitude=string.Empty,
                    TimeZone=string.Empty,
                    DmaId=string.Empty,
                    Code=string.Empty,
                    NCountryID=countrId,
                    StateID=regionId,
                    UniqueCityName = uniqueName,
                    IsInUse=false,
                    UniqueCityNameInURL=nameInUrl,
                };
                _dataContext.Cities.Add(key);
                _dataContext.SaveChanges();
                return key;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " CityCreateOrUpdate()");
            }
            return null;
        }
        public void CityUpdateIsInUseInfo(City current)
        {
            try
            {
                var dbOne = _dataContext.Cities.Where(x => x.CityId == current.CityId).SingleOrDefault();
                if (dbOne == null)
                    return;
                dbOne.IsInUse = current.IsInUse;
                _dataContext.Entry(dbOne).Property(x => x.IsInUse).IsModified = true;
                _dataContext.SaveChanges();
                HttpRuntime.Cache.Remove(cacheNameCityInUse);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " CityUpdateIsInUseInfo()");
            }
        }
        public Region RegionCreateOrGet(string region,int countryId)
        {
            try
            {
                var oldOne = _dataContext.Regions.Where(x => x.Name.ToLower() == region.ToLower() && x.CountryId == countryId).SingleOrDefault();
                if (oldOne != null)
                    return oldOne;
                Region key = new Region()
                {
                    Name = region,
                    CountryId=countryId,
                    Code=region,
                    ADM1Code=region,
                };
                _dataContext.Regions.Add(key);
                _dataContext.SaveChanges();
                return key;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " RegionCreate()");
            }
            return null;
        }
        public Country CountryCreateOrUpdate(Country key)
        {
            try
            {
                if (key.country_id > 0)
                    EnsureAttachedAndModified("Countries", key);
                else
                {
                    _dataContext.Countries.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " CountryCreateOrUpdate()");
            }
            return key;
        }
        List<SelectListItem> GetPhoneCodesSelectListItem()
        {
            return GetAllInternatinalPhoneCode().OrderBy(x=>x.Country).ToSelectList(x =>string.Format("{0}({1})",x.Country,x.AreaCode), x => x.AreaCode.ToString());
        }
        //cached queries
        public List<SelectListItem> GetPhoneCodesSelectListItemCached()
        {
            return _cacheProvider.Get("PhoneCodesSelectListItem", TimeSpan.FromDays(100), PhoneCodesSelectListCacheLock, GetPhoneCodesSelectListItem);
        }

        List<SelectListItem> GetCitiesSelectListItem()
        {
            return GetAllCitiesInUseCached().ToSelectList(x => x.Name, x => x.CityId.ToString());
        }
        //cached queries
        public List<SelectListItem> GetCitiesSelectListItemCached()
        {
            return _cacheProvider.Get("citiesSelectListItem", TimeSpan.FromDays(100), CitiesSelectListCacheLock, GetCitiesSelectListItem);
        }
        public IEnumerable<City> GetAllCitiesInUseCached()
        {
            return _cacheProvider.Get(cacheNameCityInUse, TimeSpan.FromDays(100), CitiesCacheLock, GetAllCitiesInUse);
        }
        public CitySelectorData GetCitySelectorDataCached()
        {
            return _cacheProvider.Get("citySelectorData", TimeSpan.FromDays(100), CitiesCacheLock, GetCitySelectorData);
        }
        public CitySelectorData GetCitySelectorData()
        {
            var cities = GetAllCitiesInUseCached();
            return new CitySelectorData {
                CanadaCitiesByRegion = cities.Where(x => x.NCountryID == 17).OrderBy(x => x.UniqueCityName),
                //UKCitiesByRegion = cities.Where(x => x.NCountryID == 92).GroupBy(x => x.Region).OrderBy(x => x.Key.Name),
                //USACitiesByRegionAndLetter = cities.Where(x => x.NCountryID == 1).GroupBy(x => x.Region).OrderBy(x => x.Key.Name).GroupBy(x => x.Key.Name.Substring(0, 1)),
                //USARegionLetters = cities.Where(x => x.NCountryID == 1).GroupBy(x => x.Region).GroupBy(x => x.Key.Name.Substring(0, 1)).Select(x => x.Key)
            };
        }
        public IEnumerable<Country> GetCountriesCached()
        {
            return _cacheProvider.Get("countries", TimeSpan.FromDays(100), CountriesCacheLock, GetAllCountries);
        }

        public Country GetCountryByNameCached(string countryName)
        {
            return GetCountriesCached().Where(x => x.description.ToLower() == countryName.ToLower() || x.abbreviation == countryName).FirstOrDefault();
        }
        public IEnumerable<City> GetAllCachedCitiesStartWith(int maxRows, int countryId, string startWith)
        {
            return GetAllCitiesInUseCached().Where(x => x.NCountryID == countryId && x.UniqueCityName.ToLower()
                    .StartsWith(startWith.ToLower())).Take(maxRows).ToList();
        }
        public IEnumerable<City> GetAllCachedCitiesStartWith(int maxRows, string startWith)
        {
            return GetAllCitiesInUseCached().Where(x => x.UniqueCityName.ToLower().StartsWith(startWith.ToLower())).Take(maxRows).ToList();
        }
        
        public IEnumerable<Country> GetAllCachedCountriesStartWith(int maxRows, string startWith)
        {
            return GetCountriesCached().Where(x => x.description.ToLower().StartsWith(startWith.ToLower())).Take(maxRows).ToList();
        }
        public Country GetCountryById(int? id)
        {
            if (id==null)
            {
                return null;
            }
            return _dataContext.Countries.Where(x => x.country_id == id).FirstOrDefault();
        }
        public int? GetCityIdByUrlName(string cityUrlName)
        {
            return GetAllCitiesInUseCached()
                .Where(x => x.UniqueCityNameInURL.ToUpper() == cityUrlName.ToUpper())
                .Select(x => x.CityId).SingleOrDefault();
        }
        public City GetCityOrNull(int cityId)
        {
            return GetAllCitiesInUseCached().Where(x => x.CityId == cityId).SingleOrDefault();
        }
        //public City GetCity(int cityId)
        //{
        //    return GetAllCitiesInUseCached().Where(x => x.CityId == cityId).Single();
        //}
        public City GetCityByName(string name)
        {
            var city = GetCityByNameOrNull(name);
            if(city == null)
                throw new ApplicationException("Unable to find city with name specified");

            return city;
        }
        public City GetCityByNameOrNull(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return GetAllCitiesInUseCached().Where(x => x.UniqueCityName.ToUpper() == name.ToUpper()).SingleOrDefault();
        }

        public City GetCityByNameInUrlOrNull(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return GetAllCitiesInUseCached().Where(x => x.UniqueCityNameInURL.ToLower() == name.ToLower()).SingleOrDefault();
        }

        public IEnumerable<City> GetCitiesThatStartsWith(string nameStartsWith, int maxRows)
        {
            return GetAllCitiesInUseCached().Where(x => x.UniqueCityName.ToLower().StartsWith(nameStartsWith.ToLower())).Take(maxRows).ToList();
        }

        //public City GetCityByIpOrNull(string ip)
        //{
        //    return _dataContext.NCitiesGetCityByIP(ip).SingleOrDefault();
        //}
    }

    public class CitySelectorData
    {
        public IOrderedEnumerable<City> CanadaCitiesByRegion;
        //public IOrderedEnumerable<IGrouping<Region, City>>              CanadaCitiesByRegion;
        //public IOrderedEnumerable<IGrouping<Region, City>>              UKCitiesByRegion;
        //public IEnumerable<IGrouping<string, IGrouping<Region, City>>>  USACitiesByRegionAndLetter;
        //public IEnumerable<string>                                      USARegionLetters;
    }
}
