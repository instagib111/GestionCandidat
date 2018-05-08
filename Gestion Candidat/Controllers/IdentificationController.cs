using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Candidat.Controllers
{
    public class IdentificationController : Controller
    {
        // GET: Identification
        public ActionResult Login()
        {
            return View();
        }
    }
}