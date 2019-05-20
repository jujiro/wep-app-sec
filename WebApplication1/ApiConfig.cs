using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebFormAndWebApiLab
{
    public static class ApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable attributes based routing
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "ApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}