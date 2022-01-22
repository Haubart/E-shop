using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searching, string button)
        {
            DatabaseEntities dbe = new DatabaseEntities();

            string ok = button;

            return View(dbe.ProduktTabel.Where(x => x.ProduktNavn.Contains(searching) || searching == null));
        }

        [HttpPost]
        public ActionResult Index(Kurv kurv, ProduktTabel produkt )
        {

            
            var db = new DatabaseEntities();
            produkt.ProduktNavn = kurv.ProduktNavn;
            produkt.Image = kurv.Image;
            produkt.Pris = kurv.Pris;
            return View();
        }
        public ActionResult Kurv()
        {
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }

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
                    nyModel.LoginErrorMessage = "forkert brugernavn eller adgangskode";
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
        public ActionResult Profil(Bruger model, HttpPostedFileBase profil)
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


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Support()
        {
            ViewBag.Message = "Your Shit page.";

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

        /*Nedenstående er til test*/
        [HttpPost]
        public ActionResult Login(Bruger reg)
        {
            
                var db = new DatabaseEntities();

            if (ModelState.IsValid)
            {

                if (db.Bruger.Where(x => x.Mail == reg.Mail).Any())
                {
                    //Do what do u need to do...
                }
                else
                {
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        /*ovenstående er til test*/


        public ActionResult AddImage()
        {
            ProduktTabel b1 = new ProduktTabel();
            return View(b1);
        }

        [HttpPost]
        public ActionResult AddImage(ProduktTabel model, HttpPostedFileBase image1, string searching )
        {
            DatabaseEntities db1 = new DatabaseEntities();

            var db = new DatabaseEntities();

            if (image1 != null) // hvis den ikke er lig med null
            {
                model.Image = new byte[image1.ContentLength];
                image1.InputStream.Read(model.Image, 0, image1.ContentLength);
            }
            db.ProduktTabel.Add(model);
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
    }
}