using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Reflection;
using System.Web.Routing;
using System.Web.SessionState;

namespace MVCSite.Web
{
    public class UnityControllerFactory : IControllerFactory
    {
        private readonly IUnityContainer    _container;
        private readonly Assembly           _executingAssembly;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
            _executingAssembly = Assembly.GetExecutingAssembly();
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controllerType = _executingAssembly.GetType("MVCSite.Web.Controllers." + controllerName + "Controller", false, true);
            IController currentController;
            if (controllerType != null)
            {
                currentController = (IController)_container.Resolve(controllerType);
                HttpContext.Current.Session["controllerInstance"] = currentController;
                return currentController;
            }

            var area = requestContext.RouteData.DataTokens["area"];
            controllerType = _executingAssembly.GetType(string.Format("MVCSite.Web.Areas.{0}.Controllers.{1}Controller", area, controllerName), false, true);
            if (controllerType != null)
            {
                currentController = (IController)_container.Resolve(controllerType);
                HttpContext.Current.Session["controllerInstance"] = currentController;
                return currentController;
            }

            return null;
            //throw new NullReferenceException(string.Format("Controller named {0} was not found", controllerName));
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller) { }
    }
}