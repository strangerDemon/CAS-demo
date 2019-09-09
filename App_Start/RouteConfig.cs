using System.Web.Mvc;
using System.Web.Routing;

namespace ZoneTop.Application.SSO
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");
            routes.IgnoreRoute("#/{*pathInfo}");

            //routes.RouteExistingFiles = true;

            //routes.IgnoreRoute("Content/esriMap/{filename}.html");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "cas", action = "login", id = UrlParameter.Optional }
            );
        }
    }
}