﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Gestion_Candidat.Models;
using System.IO;
using DataTables.Mvc;

namespace Gestion_Candidat.Controllers
{
    public class CandidatsController : Controller
    {
        private AcialEntities db = new AcialEntities();
        private string baseFile = @"D:\Users\InstaGib111\Documents\Chris\Doranco\bac+4\gestion_candidat\Gestion Candidat\FichierCandidat\";

        #region vue
        // GET: Candidats
        [HttpGet]
        public ActionResult Vue(string nomVue = "Chrono")
        {
            Salarie sal = db.Salarie.Find(User.Identity.GetUserId());
            var candidats = db.Candidat.Include(c => c.Humain)
                                      .Include(c => c.Salarie)
                                      .Include(c => c.Salarie1)
                                      .Include(c => c.typActionCandidat)
                                      .Include(c => c.typOrigineCandidat)
                                      .Include(c => c.typPrioriteCandidat)
                                      .Include(c => c.typStatutCandidat)
                                      .OrderByDescending(c => c.DtModification);

            if (nomVue.Contains("cdt"))
            {
                candidats = db.Candidat.Include(c => c.Humain)
                                      .Include(c => c.Salarie)
                                      .Include(c => c.Salarie1)
                                      .Include(c => c.typActionCandidat)
                                      .Include(c => c.typOrigineCandidat)
                                      .Include(c => c.typPrioriteCandidat)
                                      .Include(c => c.typStatutCandidat)
                                      .OrderBy(c => c.DtModification);
            }

            if (sal.Role1.FirstOrDefault().TypTdb == "CNS" || sal.Role1.FirstOrDefault().TypTdb == "COM")
                candidats = (IOrderedQueryable<Candidat>)candidats.Where(c => c.Role.FirstOrDefault().TypTdb == "CNS");
            else if (sal.Role1.FirstOrDefault().TypTdb == "RDD")
                candidats = (IOrderedQueryable<Candidat>)candidats.Where(c => c.Role.FirstOrDefault().TypTdb == "CNS" || c.Role.FirstOrDefault().TypTdb == "COM");
            return View(candidats.ToList());
        }
        [HttpPost]
        public ActionResult actionVue([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            IQueryable<Candidat> query = db.Candidat;
            var totalCount = query.Count();

            #region Filtering
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Mobilite.Contains(value) ||
                                         p.MCEntreprise.Contains(value) ||
                                         p.MCFonctionnel.Contains(value) ||
                                         p.MCTechnique.Contains(value) ||
                                         p.Humain.Nom.Contains(value) ||
                                         p.Humain.Prenom.Contains(value) ||
                                         p.Humain.Ville.Contains(value) ||
                                         p.Humain.Adresse.Contains(value) ||
                                         p.Humain.CodePostal.Contains(value) ||
                                         p.Humain.Pays.Contains(value) ||
                                         p.Humain.email.Contains(value)
                                   );
            }

            var filteredCount = query.Count();

            #endregion Filtering

            #region Sorting
            // Sorting
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += (column.Data) +
                  (column.SortDirection ==
                  Column.OrderDirection.Ascendant ? " asc" : " desc");

                string col = orderByString.Split(' ')[0];
                string dir = orderByString.Split(' ')[1];
                switch (col.ToLower())
                {
                    case "nom":
                        query = dir == "asc" ? query.OrderBy(c => c.Humain.Nom) : query.OrderByDescending(c => c.Humain.Nom);
                        break;
                    case "prenom":
                        query = dir == "asc" ? query.OrderBy(c => c.Humain.Prenom) : query.OrderByDescending(c => c.Humain.Prenom);
                        break;
                    case "mobilite":
                        query = dir == "asc" ? query.OrderBy(c => c.Mobilite) : query.OrderByDescending(c => c.Mobilite);
                        break;
                    case "infcom":
                        query = dir == "asc" ? query.OrderBy(c => c.InfCom) : query.OrderByDescending(c => c.InfCom);
                        break;
                    case "mctechnique":
                        query = dir == "asc" ? query.OrderBy(c => c.MCTechnique) : query.OrderByDescending(c => c.MCTechnique);
                        break;
                    case "mcfonctionnel":
                        query = dir == "asc" ? query.OrderBy(c => c.MCFonctionnel) : query.OrderByDescending(c => c.MCFonctionnel);
                        break;
                    case "mcentreprise":
                        query = dir == "asc" ? query.OrderBy(c => c.MCEntreprise) : query.OrderByDescending(c => c.MCEntreprise);
                        break;
                    default:
                        query = dir == "asc" ? query.OrderBy(c => c.DtModification) : query.OrderByDescending(c => c.DtModification);
                        break;
                }
            }

            //query = query.OrderBy(c => orderByString == string.Empty ? "DtModification desc" : orderByString);

            #endregion Sorting

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            var data = query.Select(c => new
            {
                CdCandidat = c.CdCandidat,
                DtModification = c.DtModification,
                Mobilite = c.Mobilite,
                MCEntreprise = c.MCEntreprise,
                MCFonctionnel = c.MCFonctionnel,
                MCTechnique = c.MCTechnique,
                priorite = c.typPrioriteCandidat.libele,
                statut = c.typStatutCandidat.libele,
                InfCom = c.InfCom,
                Nom = c.Humain.Nom,
                Prenom = c.Humain.Prenom,
                Ville = c.Humain.Ville,
                Adresse = c.Humain.Adresse,
                CodePostal = c.Humain.CodePostal,
                Pays = c.Humain.Pays,
                email = c.Humain.email
            }).ToList();

            return Json(new DataTablesResponse
            (requestModel.Draw, data, filteredCount, totalCount),
                        JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region MesTaches
        // GET: Candidats
        [HttpGet]
        public ActionResult MesTaches()
        {
            string current = User.Identity.GetUserId();
            var taches = db.TacheCandidat
                    .Include(c => c.Candidat)
                    .Include(c => c.Salarie)
                    .Include(c => c.Salarie1)
                    .Where(c => c.CdReceveur == current)
                    .OrderByDescending(c => c.DtAction);

            return View(taches.ToList());
        }
        [HttpPost]
        public ActionResult togTache([Bind(Include = "CdTache, Statut")]TacheCandidat tache)
        {
            TacheCandidat tc = db.TacheCandidat.Find(tache.CdTache);
            if (ModelState.IsValid)
            {
                tc.Statut = tache.Statut;
                db.SaveChanges();
                return Json(new { success = true, statut = tc.Statut }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Details
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
        #endregion
        #region Ajouter
        // GET: Candidats/Ajouter
        public ActionResult Ajouter()
        {
            List<SelectListItem> civ = new List<SelectListItem>
            {
                new SelectListItem() { Text = "", Value = "" },
                new SelectListItem() { Text = "Monsieur", Value = "M." },
                new SelectListItem() { Text = "Madame", Value = "Mme" }
            };
            ViewBag.ListeCiv = new SelectList(civ, "Value", "Text");
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
            [Bind(Include = "TypTdb")] Role roleCandidat)
        {
            int newHumain = db.Humain.Select(h => h.CdHumain).ToList().Last() + 1;
            candidat.CdHumain = newHumain;
            roleCandidat.CdHumain = newHumain;
            roleCandidat.CdCandidat = db.Candidat.Select(c => c.CdCandidat).ToList().Last() + 1;
            //role.CdRole = db.Role.Select(r => r.CdRole).ToList().Last() + 1;
            if (ModelState.IsValid)
            {
                db.Humain.Add(humain);
                db.SaveChanges();

                db.Candidat.Add(candidat);
                db.SaveChanges();

                db.Role.Add(roleCandidat);
                db.SaveChanges();
                return RedirectToAction("fiche", "Candidat", new { id = candidat.CdCandidat });
            }
            return RedirectToAction("Vue");
        }
        #endregion
        #region fiche
        // GET: Candidats/Edit/5
        public ActionResult fiche(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Vue", "Candidats");
            }
            Candidat candidat = db.Candidat.Find(id);
            if (candidat == null)
            {
                //TODO : Envoyer vers une page type
                // Ce candidat semble ne pas exister Retour à la liste
                return RedirectToAction("Vue", "Candidats");
            }
            List<SelectListItem> civ = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Monsieur", Value = "M." },
                new SelectListItem() { Text = "Madame", Value = "Mme" }
            };
            List<SelectListItem> listeResp = new List<SelectListItem>();
            foreach (var sal in db.Salarie)
            {
                if (sal.Role1.ElementAt(0).IsResp == true)
                    listeResp.Add(new SelectListItem() { Text = sal.Humain.Prenom + " " + sal.Humain.Nom + " (" + sal.CdSalarie + ")", Value = sal.CdSalarie });
            }
            List<SelectListItem> listeFichier = new List<SelectListItem>();
            foreach (var fic in db.typFichierCandidat)
                listeFichier.Add(new SelectListItem() { Text = fic.libele, Value = fic.cdFichier.ToString() });

            Salarie currentSalarie = db.Salarie.Find(User.Identity.GetUserId());
            ViewBag.isASS = currentSalarie.Role1.First().TypTdb == "ASS";
            string user = User.Identity.GetUserId();
            ViewBag.User = user;

            ViewBag.ListeResp = listeResp;
            ViewBag.ListeFichier = listeFichier;
            ViewBag.ListeTypEvent = db.typEvenementCandidat;
            ViewBag.ListeCiv = new SelectList(civ, "Value", "Text", candidat.Humain.Civilite);
            ViewBag.TypAction = new SelectList(db.typActionCandidat, "cdAction", "libele", candidat.TypAction);
            ViewBag.TypOrigine = new SelectList(db.typOrigineCandidat, "cdOrigine", "libele", candidat.TypOrigine);
            ViewBag.TypPriorite = new SelectList(db.typPrioriteCandidat, "cdPriorite", "libele", candidat.TypPriorite);
            ViewBag.TypStatut = new SelectList(db.typStatutCandidat, "cdStatut", "libele", candidat.TypStatut);
            ViewBag.TypTdb = new SelectList(db.typTdb, "cdTdb", "libele", candidat.Role.First().TypTdb);

            return View(candidat);
        }

        // POST: Candidats/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fiche([Bind(Include =
            "CdHumain,CdCandidat,DtDisponibilite,LbDisponibilite,Remuneration,Mobilite,InfCom," +
            "TypAction,TypPriorite,TypOrigine,TypStatut," +
            "MCEntreprise,MCFonctionnel,MCTechnique," +
            "DtCreation,CreePar,DtModification,ModifiePar")] Candidat candidat,
            [Bind(Include = "CdHumain,Civilite,Prenom,Nom,TelMobile,email," +
            "Adresse,AdresseComplement,CodePostal,Ville,Pays")] Humain humain,
            [Bind(Include = "CdRole,TypTdb")] Role roleCandidat, ICollection<EvenementCandidat> EvenementCandidat)
        {
            roleCandidat.CdHumain = humain.CdHumain;
            roleCandidat.CdCandidat = candidat.CdCandidat;
            roleCandidat.CdRole = db.Role
                .Where(r => r.CdHumain == humain.CdHumain)
                .Select(r => r.CdRole).ToList().First();
            if (ModelState.IsValid)
            {
                db.Entry(humain).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(roleCandidat).State = EntityState.Modified;
                db.SaveChanges();
                if (EvenementCandidat != null)
                {

                    foreach (var evt in EvenementCandidat)
                    {
                        var temp = db.EvenementCandidat.Where(e => e.CdEvenement == evt.CdEvenement).First();
                        mapEvenememtCandidat(temp, evt);
                    }
                    db.SaveChanges();
                }
                db.Entry(candidat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Vue");
            }

            return RedirectToAction("fiche", "Candidat", new { id = candidat.CdCandidat });
        }
        private void mapEvenememtCandidat(EvenementCandidat temp, EvenementCandidat newCnd)
        {
            temp.CdResp = newCnd.CdResp;
            temp.CommentaireEvenement = newCnd.CommentaireEvenement;
            temp.DtEvenement = newCnd.DtEvenement;
            temp.TypEvenement = newCnd.TypEvenement;

            db.EvenementCandidat.Attach(temp);
            var entry = db.Entry(temp);
            entry.Property(e => e.CdResp).IsModified = true;
            entry.Property(e => e.CommentaireEvenement).IsModified = true;
            entry.Property(e => e.DtEvenement).IsModified = true;
            entry.Property(e => e.TypEvenement).IsModified = true;
        }
        #endregion
        #region Supprimer
        // GET: Candidats/Delete/5
        public ActionResult supp(int? id)
        {
            Salarie currentSalarie = db.Salarie.Find(User.Identity.GetUserId());
            if (currentSalarie.Role.First().TypTdb != "ASS")
                return RedirectToAction("Vue");
            if (id == null)
                return RedirectToAction("Vue");
            Candidat candidat = db.Candidat.Find(id);
            Role role = db.Role.Where(r => r.CdCandidat == id).First();
            if (currentSalarie.Role.First().TypTdb == "ASS")
            {
                db.Role.Remove(role);
                List<TacheCandidat> lt = candidat.TacheCandidat.ToList();
                foreach (var tache in lt)
                    db.TacheCandidat.Remove(tache);
                List<EvenementCandidat> le = candidat.EvenementCandidat.ToList();
                foreach (var evt in le)
                    db.EvenementCandidat.Remove(evt);
                List<FichierCandidat> lf = candidat.FichierCandidat.ToList();
                foreach (var fichier in lf)
                    suppDoc(fichier);
                suppAllFav(candidat);
                db.Candidat.Remove(candidat);
                db.SaveChanges();
            }
            return RedirectToAction("Vue");
        }
        #endregion
        #region method
        [HttpPost]
        public ActionResult ajouterTache(
            [Bind(Include = "CdCandidat, CdReceveur, DtAction, Details")]
            TacheCandidat tache)
        {
            tache.DtEnvoi = DateTime.Now;
            tache.Statut = false;
            tache.CdCreateur = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.TacheCandidat.Add(tache);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ajouterDoc(
            [Bind(Include = "CdCandidat, TypFichier, Nom, LienPath")]
            FichierCandidat fichier, HttpPostedFileBase doc)
        {
            fichier.DtCreation = DateTime.Now;
            fichier.DtModification = DateTime.Now;
            fichier.LienPath = doc.FileName;
            if (ModelState.IsValid)
            {
                try
                {
                    if (doc != null && doc.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(doc.FileName);
                        var path = Path.Combine(baseFile, fileName);
                        doc.SaveAs(path);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { success = false, responseText = "Une erreur s'est produite lors du téléchargement du fichier" }, JsonRequestBehavior.AllowGet);
                }

                db.FichierCandidat.Add(fichier);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult suppDoc([Bind(Include = "CdFichier")]FichierCandidat fichier)
        {
            string res = "ok";
            if (ModelState.IsValid)
            {
                FichierCandidat fic = db.FichierCandidat.Find(fichier.CdFichier);
                if (System.IO.File.Exists(baseFile + fic.LienPath))
                    System.IO.File.Delete(baseFile + fic.LienPath);
                else
                    res = "Fichier non trouvé, impossible de le supprimer";
                db.FichierCandidat.Remove(fic);
                db.SaveChanges();
                return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ajouterEvent(
            [Bind(Include = "CdCandidat, CdResp, TypEvenement, DtEvenement, CommentaireEvenement")]
            EvenementCandidat evenement)
        {
            evenement.DtCreation = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.EvenementCandidat.Add(evenement);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult suppEvent([Bind(Include = "CdEvenement")] EvenementCandidat evenement)
        {
            if (ModelState.IsValid)
            {
                EvenementCandidat evt = db.EvenementCandidat.Find(evenement.CdEvenement);
                db.EvenementCandidat.Remove(evt);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFav([Bind(Include = "CdCandidat, CdSalarie")] FavorisCandidat fav)
        {
            int cdfav = -1;
            if (ModelState.IsValid)
            {
                try
                {
                    cdfav = db.FavorisCandidat.Where(f => f.CdCandidat == fav.CdCandidat && f.CdSalarie == fav.CdSalarie).First().CdFavoris;
                    return Json(new { success = true, responseText = cdfav }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    cdfav = -1;
                    return Json(new { success = false, responseText = cdfav }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = cdfav }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult addFav([Bind(Include = "CdCandidat, CdSalarie")] FavorisCandidat fav)
        {
            if (ModelState.IsValid)
            {
                FavorisCandidat fc = db.FavorisCandidat.Add(fav);
                db.SaveChanges();
                int cdfav = fc.CdFavoris;
                return Json(new { success = true, responseText = cdfav }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult suppFav([Bind(Include = "CdFavoris")] FavorisCandidat fav)
        {
            if (ModelState.IsValid)
            {

                db.FavorisCandidat.Remove(db.FavorisCandidat.Find(fav.CdFavoris));
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult suppAllFav([Bind(Include = "CdCandidat")] Candidat candidat)
        {
            if (ModelState.IsValid)
            {
                List<FavorisCandidat> lf = db.FavorisCandidat.Where(f => f.CdCandidat == candidat.CdCandidat).ToList();
                foreach (var doc in lf)
                    db.FavorisCandidat.Remove(doc);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "model invalid" }, JsonRequestBehavior.AllowGet);
        }
        public FileResult telecharger(string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(baseFile + fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion
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
