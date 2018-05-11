using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Candidat.Models;

namespace Gestion_Candidat.Controllers
{
    public class AcialController : Controller
    {
        private AcialEntities dba = new AcialEntities();

        public ActionResult Accueil()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "les contacts de l'entreprise";

            var contact = dba.Salarie.Include(c => c.Humain);

            return View(contact.ToList());
        }
    }
}