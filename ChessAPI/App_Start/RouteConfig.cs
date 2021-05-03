using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChessAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{userID}/{id}/{move}",
                defaults: new { controller = "Home", action = "Index",
                    userID = UrlParameter.Optional,
                    id = UrlParameter.Optional,
                    move = UrlParameter.Optional
                }
            );
        }
    }
}
