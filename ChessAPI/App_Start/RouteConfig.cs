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
                name: "Default4",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"
                }
            );
            routes.MapRoute(
                name: "Default3",
                url: "{controller}/{action}/{userID}",
                defaults: new { controller = "Home", action = "Index",
                    userID = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{name}/{*password}",
                defaults: new
                {
                    //controller = "Players",
                    action = "Index",
                    name = UrlParameter.Optional,
                    password = UrlParameter.Optional
                }
            );
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
