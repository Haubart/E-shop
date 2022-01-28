using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.ViewModel;

namespace E_shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(E_shop.Models.Bruger nyModel)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode).FirstOrDefault();
                if (brugerOplysninger == null)
                {
                    nyModel.LoginErrorMessage = "Forkert brugernavn eller adgangskode";
                    return View("Login", nyModel);
                }
                else
                {
                    Session["userID"] = brugerOplysninger.BrugerID;
                    Session["ForNavn"] = brugerOplysninger.ForNavn;
                    Session["EfterNavn"] = brugerOplysninger.EfterNavn;
                    Session["Adgangskode"] = brugerOplysninger.Adgangskode;
                    Session["Mail"] = brugerOplysninger.Mail;
                    Session["Adresse"] = brugerOplysninger.Adresse;
                    Session["Postnr"] = brugerOplysninger.Postnr;
                    Session["By"] = brugerOplysninger.By;
                    Session["Telefon"] = brugerOplysninger.Telefon;
                    Session["Land"] = brugerOplysninger.Land;
                    return View("efterLogin");
                }                
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult efterLogin()
        {
            return View();
        }

        public ActionResult Profil()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Profil(Bruger nyModel)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode).FirstOrDefault();
                if (brugerOplysninger == null)
                {
                    nyModel.LoginErrorMessage = "Kunne ikke opdatere oplysninger grundet forkert Email eller adgangskode";
                    return View("Profil", nyModel);
                }
                else
                {
                    brugerOplysninger.ForNavn = nyModel.ForNavn;
                    brugerOplysninger.EfterNavn = nyModel.EfterNavn;
                    brugerOplysninger.Adresse = nyModel.Adresse;
                    brugerOplysninger.Postnr = nyModel.Postnr;
                    brugerOplysninger.By = nyModel.By;
                    brugerOplysninger.Telefon = nyModel.Telefon;
                    brugerOplysninger.Land = nyModel.Land;
                    db.SaveChanges();
                    return View("OpdateringAfOplysninger");
                }
            }
        }

        public ActionResult OpdateringAfOplysninger()
        {
            return View();
        }

        public ActionResult Support()
        {
            return View();
        }

        public ActionResult Opret()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Opret(Bruger model, HttpPostedFileBase profil)
        {
            var db = new DatabaseEntities();
            db.Bruger.Add(model);
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return View(model);
        }

        public ActionResult Checkout()
        {
            if (Session["userID"] != null)
            {
                return View("Checkoutdata");
            }
            else
            {
                return View("Checkoutnodata");
            }
        }


        [HttpPost]
        public ActionResult Checkoutdata(Bruger nyModel)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode).FirstOrDefault();
                if (brugerOplysninger == null)
                {
                    nyModel.LoginErrorMessage = "Kunne ikke gennemføre købet grundet forkert Email eller adgangskode";
                    return View("Checkoutdata", nyModel);
                }
                else
                {
                    return View("KøbGennemført");
                }
            }
        }


        [HttpPost]
        public ActionResult Checkoutnodata()
        {
            return View("KøbGennemført");
        }


        public ActionResult KøbGennemført()
        {
            return View();
        }


    }
}