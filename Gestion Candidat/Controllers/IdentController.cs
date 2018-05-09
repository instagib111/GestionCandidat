using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Gestion_Candidat.Models;
using Microsoft.AspNet.Identity;

namespace Gestion_Candidat.Controllers
{
    public class IdentController : Controller
    {
        private AcialEntities db = new AcialEntities();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(IDENTIFIANTSALARIE model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            try
            {
                model.cdId = db.IDENTIFIANTSALARIE.Where(x => x.cdSalarie == model.cdSalarie).First().cdId;
                model.Salarie = db.Salarie.Where(x => x.CdSalarie == model.cdSalarie).First();
                model.Salarie.Humain = db.Humain.Where(x => x.CdHumain == model.Salarie.CdHumain).First();
                //model.Salarie.Humain.Role = db.Role.Where(x => x.CdHumain == model.Salarie.CdHumain).First();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
            }
            if(model.cdId != 0)
            {
                ModelState["cdId"].Errors.Clear();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!ValidateUser(model.cdSalarie, model.key))
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }

            // L'authentification est réussie, 
            // injecter l'identifiant utilisateur dans le cookie d'authentification :
            var loginClaim = new List<Claim>();
            loginClaim.Add(new Claim(ClaimTypes.NameIdentifier, model.Salarie.Humain.Prenom + " " + model.Salarie.Humain.Nom));
            loginClaim.AddRange(LoadRoles("Associé"));
            var claimsIdentity = new ClaimsIdentity(loginClaim, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity);

            // Rediriger vers l'URL d'origine :
            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                return Redirect(ViewBag.ReturnUrl);
            // Par défaut, rediriger vers la page d'accueil :
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<Claim> LoadRoles(string role)
        {
            yield return new Claim(ClaimTypes.Role, role);
            //TODO :  arranger cette méthode
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var ident = ctx.Authentication;
            ident.SignOut();
            return RedirectToAction("Accueil", "Acial");
        }

        private bool ValidateUser(string cdSalarie, string mdp)
        {
            bool ok = false;
            var co = db.IDENTIFIANTSALARIE.Where(x => x.cdSalarie == cdSalarie).First();
            if (co.cdId >= 1 && co.key == mdp)
                ok = true;
            return ok;   
        }

    }
}