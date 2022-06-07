using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MyMvc5App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

#if DEBUG
            //var corsAttr = new EnableCorsAttribute("http://localhost:16909", "*", "*");
            var corsAttr = new EnableCorsAttribute("http://localhost:4200", "*", "*");
#else
            var corsAttr = new EnableCorsAttribute("http://rodansotto.com", "*", "*");
#endif
            config.EnableCors(corsAttr);
        }
    }
}
