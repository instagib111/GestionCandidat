using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gestion_Candidat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "VueRecherche",
                url: "{controller}/{action}/{vue}/{recherche}",
                defaults: new { controller = "Candidat", action = "Recherche" },
                constraints: new { action = "recherche", vue = @"\w{3}+", recherche = @"\w{2}+" }
            );

            routes.MapRoute(
                name: "Recherche",
                url: "{controller}/{action}/{recherche}",
                defaults: new { controller = "Candidat", action = "Recherche" },
                constraints: new { action = "recherche" , recherche = @"\w{2}+"}
            );

            routes.MapRoute(
                name: "FicheCandidat",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Candidat", action = "Fiche" },
                constraints: new { controller = "candidat", action = "fiche", id = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Acial", action = "Accueil" }
            );
        }
    }
}
