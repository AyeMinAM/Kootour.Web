using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using MVCSite.DAC.Common;

namespace MVCSite.Web.Extensions
{
    public class ExtendedRazorViewEngine : RazorViewEngine
    {
        public void AddViewLocationFormat(string paths)
        {
            var existingPaths = new List<string>(ViewLocationFormats) {paths};

            ViewLocationFormats = existingPaths.ToArray();
        }

        public void AddPartialViewLocationFormat(string paths)
        {
            var existingPaths = new List<string>(PartialViewLocationFormats) {paths};

            PartialViewLocationFormats = existingPaths.ToArray();
        }
        protected override IView CreatePartialView( ControllerContext controllerContext, string partialPath )
        {
            return base.CreatePartialView(controllerContext, partialPath);
        }
 
        protected override IView CreateView( ControllerContext controllerContext, string viewPath, string masterPath )
        {
            return base.CreateView( controllerContext, viewPath, masterPath );
        }

        //public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache);
        //{
        //    viewPath = PViewPath(controllerContext, viewPath);
        //    return base.CreateView( controllerContext, viewPath, masterPath );
        //}
        //public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache);
        //{
        //    viewPath = PViewPath(controllerContext, viewPath);
        //    return base.CreateView( controllerContext, viewPath, masterPath );
        //}

        private static string GlobalizeViewPath( ControllerContext controllerContext, string viewPath )
        {
            var request = controllerContext.HttpContext.Request;
            var lang = request.Cookies["language"];
            if( lang != null &&
                !string.IsNullOrEmpty(lang.Value) &&
                !string.Equals( lang.Value, "en", StringComparison.InvariantCultureIgnoreCase ) )
            {
                string localizedViewPath = Regex.Replace(
                    viewPath,
                    "^~/Views/",
                    string.Format( "~/Views/Globalization/{0}/",
                    lang.Value
                    ) );
                if( File.Exists( request.MapPath( localizedViewPath ) ) )
                { viewPath = localizedViewPath; }
            }
            return viewPath;
        }
        private static string PViewPath(ControllerContext controllerContext, string viewPath)
        {
            if (!viewPath.Contains("PViews"))
                return viewPath.Replace("Views", "PViews");
            else
                return viewPath;
        }

    }
}