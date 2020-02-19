using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSite.DAC.Services;
using MVCSite.Biz;
using MVCSite.DAC.Entities;
using MVCSite.Common;


namespace MVCSite.Web.Controllers 
{
    public class Base : Controller 
    {
        protected ActionResult RedirectToPage(WebsitePage page)
        {
            if (!string.IsNullOrEmpty(page.PlainUrl))
                return Redirect(page.PlainUrl);

            if (page.Area != null)
                return RedirectToAction(page.ActionName, page.ControllerName, new { area = page.Area }); //TODO: implement handling of parameters

            return RedirectToAction(page.ActionName, page.ControllerName, page.Parameters);
        }
        private void RemoveCache()
        {
            var urlToRemove = Url.Action("Index", "Board");
            Response.RemoveOutputCacheItem(urlToRemove);
        }
        protected string GetPageUrl(WebsitePage page) 
        {
            if (!string.IsNullOrEmpty(page.PlainUrl))
                return page.PlainUrl;

            if (page.Area != null)
                return Url.Action(page.Name, page.ControllerName, new { area = page.Area });

            return Url.Action(page.Name, page.ControllerName);
        }
        protected string RenderRazorViewToString(string viewName, object model) 
        {
            ViewData.Model = model;
            using (var sw = new StringWriter()) {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        protected static List<SelectListItem> UserGetRoleOptions(bool withEmptyItem = false)
        {
            var items = new List<SelectListItem>();
            if (withEmptyItem)
            {
                items.Add(new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
            }
            for (var item = UserRole.Guider; item <= UserRole.Agent; item++)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = ((short)item).ToString()
                });
            }
            return items;
        }
    }
}