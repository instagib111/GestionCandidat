using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Gestion_Candidat.Models;

namespace Gestion_Candidat.Controllers
{
    public class CandidatsController : Controller
    {
        private AcialEntities2 db = new AcialEntities2();

        // GET: Candidats
        public ActionResult Vue()
        {
            var candidat = db.Candidat.Include(c => c.Humain).Include(c => c.Salarie).Include(c => c.Salarie1).Include(c => c.typActionCandidat).Include(c => c.typOrigineCandidat).Include(c => c.typPrioriteCandidat).Include(c => c.typStatutCandidat);
            return View(candidat.ToList());
        }

        // GET: Candidats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidat candidat = db.Candidat.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            return View(candidat);
        }

        // GET: Candidats/Ajouter
        public ActionResult Ajouter()
        {
            List<SelectListItem> civ = new List<SelectListItem>();
            civ.Add(new SelectListItem() { Text = "", Value = "" });
            civ.Add(new SelectListItem() { Text = "Monsieur", Value = "M." });
            civ.Add(new SelectListItem() { Text = "Madame", Value = "Mme" });
            ViewBag.ListeCiv = civ;
            ViewBag.User = User.Identity.GetUserId();
            ViewBag.CreePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial");
            ViewBag.ModifiePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial");
            ViewBag.TypAction = new SelectList(db.typActionCandidat, "cdAction", "libele");
            ViewBag.TypOrigine = new SelectList(db.typOrigineCandidat, "cdOrigine", "libele");
            ViewBag.TypPriorite = new SelectList(db.typPrioriteCandidat, "cdPriorite", "libele");
            ViewBag.TypStatut = new SelectList(db.typStatutCandidat, "cdStatut", "libele");
            ViewBag.TypTdb = new SelectList(db.typTdb, "cdTdb", "libele");
            return View();
        }

        // POST: Candidats/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ajouter([Bind(Include = 
            "DtDisponibilite,LbDisponibilite,Remuneration,Mobilite,InfCom," +
            "TypAction,TypPriorite,TypOrigine,TypStatut," +
            "MCEntreprise,MCFonctionnel,MCTechnique," +
            "DtCreation,CreePar,DtModification,ModifiePar")] Candidat candidat, 
            [Bind(Include = "Civilite,Prenom,Nom,TelMobile,email," +
            "Adresse,AdresseComplement,CodePostal,Ville,Pays")] Humain humain,
            [Bind(Include = "TypTdb")] Role role)
        {
            int newCan = db.Candidat.Select(c => c.CdCandidat).ToList().Last() + 1;
            //candidat.CdCandidat = newCan;
            int newHumain = db.Humain.Select(h => h.CdHumain).ToList().Last() + 1;
            //humain.CdHumain = newHumain;
            candidat.CdHumain = newHumain;
            role.CdHumain = newHumain;
            //role.CdRole = db.Role.Select(r => r.CdRole).ToList().Last() + 1;
            if (ModelState.IsValid)
            {
                db.Humain.Add(humain);
                db.SaveChanges();

                db.Role.Add(role);
                db.SaveChanges();

                db.Candidat.Add(candidat);
                db.SaveChanges();
                return RedirectToAction("Vue");
            }

            //ViewBag.CdHumain = new SelectList(db.Humain, "CdHumain", "Civilite", candidat.CdHumain);
            ViewBag.CreePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.CreePar);
            ViewBag.ModifiePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.ModifiePar);
            ViewBag.TypAction = new SelectList(db.typActionCandidat, "cdAction", "libele", candidat.TypAction);
            ViewBag.TypOrigine = new SelectList(db.typOrigineCandidat, "cdOrigine", "libele", candidat.TypOrigine);
            ViewBag.TypPriorite = new SelectList(db.typPrioriteCandidat, "cdPriorite", "libele", candidat.TypPriorite);
            ViewBag.TypStatut = new SelectList(db.typStatutCandidat, "cdStatut", "libele", candidat.TypStatut);
            return View(candidat);
        }

        // GET: Candidats/Edit/5
        public ActionResult fiche(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidat candidat = db.Candidat.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            ViewBag.CdHumain = new SelectList(db.Humain, "CdHumain", "Civilite", candidat.CdHumain);
            ViewBag.CreePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.CreePar);
            ViewBag.ModifiePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.ModifiePar);
            ViewBag.TypAction = new SelectList(db.typActionCandidat, "cdAction", "libele", candidat.TypAction);
            ViewBag.TypOrigine = new SelectList(db.typOrigineCandidat, "cdOrigine", "libele", candidat.TypOrigine);
            ViewBag.TypPriorite = new SelectList(db.typPrioriteCandidat, "cdPriorite", "libele", candidat.TypPriorite);
            ViewBag.TypStatut = new SelectList(db.typStatutCandidat, "cdStatut", "libele", candidat.TypStatut);
            return View(candidat);
        }

        // POST: Candidats/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CdCandidat,CdHumain,DtDisponibilite,LbDisponibilite,Remuneration,Mobilite,TypAction,TypPriorite,TypOrigine,TypStatut,InfCom,MCEntreprise,MCFonctionnel,MCTechnique,DtCreation,CreePar,DtModification,ModifiePar")] Candidat candidat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CdHumain = new SelectList(db.Humain, "CdHumain", "Civilite", candidat.CdHumain);
            ViewBag.CreePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.CreePar);
            ViewBag.ModifiePar = new SelectList(db.Salarie, "CdSalarie", "NoSecuSocial", candidat.ModifiePar);
            ViewBag.TypAction = new SelectList(db.typActionCandidat, "cdAction", "libele", candidat.TypAction);
            ViewBag.TypOrigine = new SelectList(db.typOrigineCandidat, "cdOrigine", "libele", candidat.TypOrigine);
            ViewBag.TypPriorite = new SelectList(db.typPrioriteCandidat, "cdPriorite", "libele", candidat.TypPriorite);
            ViewBag.TypStatut = new SelectList(db.typStatutCandidat, "cdStatut", "libele", candidat.TypStatut);
            return View(candidat);
        }

        // GET: Candidats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidat candidat = db.Candidat.Find(id);
            if (candidat == null)
            {
                return HttpNotFound();
            }
            return View(candidat);
        }

        // POST: Candidats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Candidat candidat = db.Candidat.Find(id);
            db.Candidat.Remove(candidat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
