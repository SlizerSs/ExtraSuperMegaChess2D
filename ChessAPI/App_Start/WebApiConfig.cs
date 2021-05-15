using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ChessAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default4Api",
                routeTemplate: "api/{controller}",
                defaults: new
                {
                    
                }
            );
            config.Routes.MapHttpRoute(
                name: "Default3Api",
                routeTemplate: "api/{controller}/{userID}",
                defaults: new {
                    userID = RouteParameter.Optional
                }
            );
            config.Routes.MapHttpRoute(
                name: "Default2Api",
                routeTemplate: "api/{controller}/{name}/{*password}",
                defaults: new
                {
                    name = RouteParameter.Optional,
                    password = RouteParameter.Optional
                }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{userID}/{id}/{move}",
                defaults: new {
                    userID = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    move = RouteParameter.Optional
                }
            );

        }
    }
}
