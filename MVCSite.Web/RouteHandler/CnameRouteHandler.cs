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
using System.Web.Mvc;
using System.Web.Routing;
using MVCSite.Web.HttpHandler;


namespace MVCSite.Web.RouteHandler
{
    public class CnameRouteHandler : MvcRouteHandler
    {
        //private bool IsCname=false;
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CnameHandler();
        }

    }
    
}
