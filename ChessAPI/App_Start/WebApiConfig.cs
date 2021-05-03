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
