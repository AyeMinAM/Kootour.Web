using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVCSite.Web.Extensions
{
    public class CustomModelBinder : DefaultModelBinder
    {
        //private readonly Func<IRepositoryCities> _repositoryCitiesFactory;

        //public CustomModelBinder(Func<IRepositoryCities> repositoryCitiesFactory)
        //{
        //    _repositoryCitiesFactory = repositoryCitiesFactory;
        //}

        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    if (bindingContext.ModelType == typeof(DealsFilterInfo))
        //    {
        //        var initializedFilter = controllerContext.RouteData.Values.Values.OfType<DealsFilterInfo>().FirstOrDefault();
        //        //var initializedFilter = routeValues.Select(x => x.Value).OfType<DealsFilterInfo>().SingleOrDefault();
        //        if (initializedFilter != null)
        //            return initializedFilter;

        //        var routeValues = RouteHelpers.GetRouteValues(new HttpContextWrapper(HttpContext.Current));
        //        var dealsFilter =  new DealsFilterInfo {
        //            CategoryId = ConversionExtensions.TryParseIntOrNull(routeValues["category"] as string),
        //        };
                
        //        var cityUrlName = routeValues["city"] as string;
        //        if(!string.IsNullOrEmpty(cityUrlName))
        //        {
        //            var repository = _repositoryCitiesFactory();
        //            dealsFilter.CityId = repository.GetCityIdByUrlName(cityUrlName);
        //        }
        //        return dealsFilter;
        //    }
        //    if (bindingContext.ModelType == typeof(SortInfo))
        //    {
        //        var routeValues = RouteHelpers.GetRouteValues(new HttpContextWrapper(HttpContext.Current));
        //        var sortInfo = new SortInfo { Direction = SortDirection.Ascending };
        //        var sortDir = routeValues["sortDir"] as string;
        //        if (!string.IsNullOrEmpty(sortDir) && sortDir.ToUpper() == "DESC")
        //            sortInfo.Direction = SortDirection.Descending;

        //        sortInfo.Field = routeValues["sortBy"] as string;

        //        return sortInfo;
        //    }

        //    return base.BindModel(controllerContext, bindingContext);
        //}
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(HttpPostedFileBase))
            {
                return new HttpContextWrapper(HttpContext.Current);
            }

            return base.BindModel(controllerContext, bindingContext);
        }

    }
}