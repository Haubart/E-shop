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
        //Her returneres Login viewet, som er den side man logger ind på.
        public ActionResult Login()
        {
            return View();
        }

        //Her angives at det en HttpPost action idet vi skal snakke med databasen og tjekke forskellige informationer.
        [HttpPost]
        //Vi angiver, at vi vil bruge "Bruger" tabellen fra vores database, og vi medgiver en parameter "nyModel", som vi kan bruge til, at tilgå vores get og set metoder fra Bruger modellen.
        public ActionResult Autherize(Bruger nyModel)
        {
            //Vi bruger vores connectionstring til at lave et nyt obejt "db" så vi kan tilgå vores database.
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //Vi opretter en variabel som vi sætter lig med et lambda udtryk.
                /*Lambda udtrykket går ind og søger i vores database om der er en Mail der er lig med den mail der står i formen fra login viewet, 
                og om der er en adgangskode i vores database som svarer til den brugeren har angivet i formen.*/
                //Hvis ikke der er noget match er "brugerOplysninger" lig med null og der vises en errormessage og der returneres login viewet og nyModel.
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode).FirstOrDefault();
                if (brugerOplysninger == null)
                {
                    nyModel.LoginErrorMessage = "Forkert brugernavn eller adgangskode";
                    return View("Login", nyModel);
                }
                //Hvis den derimod ikke er lig med null, og der er en mail og adgangskode der stemmer overens i databasen så gemmes alle oplysningerne i en Session.
                //Det er muligt idet vi sætte "brugerOplysninger" lig med "db.Bruger" som en del af dens værdi. Derfor kender den nu til alle den brugers oplysninger hvis mail og adgangskode lige er blevet tastet ind
                //Vi gemmer som sagt oplysningerne i en Session som udløber 2 timer efter login (se webconfig).
                //Vi kan nu bruge denne session til at display værdierne på f.eks. "Profil" siden og navnet på brugeren under profil ikonet i navbaren
                //Til sidst returnes viewet "efterLogin"
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

        //Denne actionresult køres når man trykker på logud knappen Profil siden, eller andre steder på hjemmesiden. Den afslutter sessionen for brugeren og redirecter brugeren til Login siden.
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        //Her returneres efterLogin viewet.
        public ActionResult efterLogin()
        {
            return View();
        }

        //Her returneres Profil viewet.
        public ActionResult Profil()
        {
            return View();
        }

        //Under Profil siden er det muligt at ændre sine oplysninger og nedenstående HttpPost gør dette.
        [HttpPost]
        //Vi angiver, at vi vil bruge "Bruger" tabellen fra vores database, og vi medgiver en parameter "nyModel", som vi kan bruge til, at tilgå vores get og set metoder fra Bruger modellen.
        public ActionResult Profil(Bruger nyModel)
        {
            //Vi bruger vores connectionstring til at lave et nyt obejt "db" så vi kan tilgå vores database.
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //Dette lambda udtryk fungere på sammme måde som oppe i "Autherize"
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode).FirstOrDefault();
                //Hvis "brugerOplysninger" er lig med null skrives en errormessage som kaldes fra Bruger.cs via nyModel.
                //Der returneres også Profil viewet samt parameteren nyModel.
                if (brugerOplysninger == null)
                {
                    nyModel.LoginErrorMessage = "Kunne ikke opdatere oplysninger grundet forkert Email eller adgangskode";
                    return View("Profil", nyModel);
                }
                //Hvis "brugerOplysninger" ikke er lig med null og man derfor har skrevet dem korrekt ind samt skrevet sine nye oplysninger ind, så vil de blive opdateret i databasen herunder
                //Vi sætter nemlig vores "brugerOplysninger" lig med hver af de værdier der er står indskrevet i formen fra viewet som nyModel indeholder.
                //Herefter gemmer vi ændringerne og viewet "OpdateringAfOplysninger" returneres.
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

        //Her returneres OpdateringAfOplysninger viewet.
        public ActionResult OpdateringAfOplysninger()
        {
            return View();
        }

        //Her returneres Support viewet.
        public ActionResult Support()
        {
            return View();
        }

        //Her returneres Opret viewet.
        public ActionResult Opret()
        {
            return View();
        }

        //Dette er en HttpPost idet den skal poste noget ind i databasen.
        [HttpPost]
        public ActionResult Opret(Bruger model)
        {
            //Laver en Objekt af connectionstringen så databasen kan tilgåes derigennem.
            var db = new DatabaseEntities();
            //Tilføjer modellen fra "opret" viewet ved funktionen Add til Bruger-tabellen
            db.Bruger.Add(model);
            //Ændringerne i databasen gemmes med SaveChanges() funktionen
            db.SaveChanges();
            //Returnere viewet opret
            return View();
        }

        //Dette ActrionResult "Checkout" køres når man trykker på trykker på knappen "Gå til kassen" under ShoppingCart viewet.
        //Den tjekker om man er logget ind eller ej. Er man logget ind er userID ikke lig med nul og derfor redirectes man til checkout viewet med indskrevet oplysninger på forhånd
        //Er man ikke logget ind redirectes man til det checkout view uden indskrevne oplysninger.
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

        //Denne post bruges under checkoutsiden hvor dataene for brugeren er indskrevet på forhånd, hvilket de er hvis brugere er logget ind.
        [HttpPost]
        //Vi angiver, at vi vil bruge "Bruger" tabellen fra vores database, og vi medgiver en parameter "nyModel", som vi kan bruge til, at tilgå vores get og set metoder fra Bruger modellen.
        public ActionResult Checkoutdata(Bruger nyModel)
        {
            //Vi bruger vores connectionstring til at lave et nyt obejt "db" så vi kan tilgå vores database.
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //Vi opretter en variabel som vi sætter lig med et lambda udtryk.
                /*Lambda udtrykket går ind og søger i vores database om den/det Mail, adgangskode, Fornavn, Efternavn, Adresse, Postnr, By, Telefon og Land der er skrevet ind som oplysninger stemmer overens med de værdier databasen har om den bruger der er logget ind.*/
                var brugerOplysninger = db.Bruger.Where(x => x.Mail == nyModel.Mail && x.Adgangskode == nyModel.Adgangskode && x.ForNavn == nyModel.ForNavn && x.EfterNavn == nyModel.EfterNavn && x.Adresse == nyModel.Adresse && x.Postnr == nyModel.Postnr && x.By == nyModel.By && x.Telefon == nyModel.Telefon && x.Land == nyModel.Land).FirstOrDefault();

                //Herunder sætter vi en string for Fornavn lig med den tilhørende værdi fra formen i viewet.
                string Fornavn = nyModel.ForNavn;
                //Årsagen til dette er at vi vil sikre os at alle felter er udfyldte, og brugeren derfor ikke bare sletter alle værdier og dermed kan gå videre til den næste side.
                //Det der sikrer dette er if-statementet herunder. Den tjekker både om "brugerOplysninger" er lig null og om der er en værdi i feltet med Fornavn. For hvis alle værdier er slettet er der jo ingen værdi i Fornavn.
                //Derfor behøver vi ikke tjekke på alle andre felter, men behøver kun ét.
                //If-statementet er skrevet med en or som boolsk opperator idet kun en af tingene skal være true før vi sætter ind, og skriver en errormessage.
                //For hvis ikke alle indskrevne værdier stemmer overnes med dem i databasen skal der komme en fejl. Eller hvis Foravn er tomt skal der også komme en fejl.
                //HØjre side af if-statementet tjekker overordnet faktisk om alle felte er tomme. Venstre side af af if-statementet kan nemlig tjekke om værdierne passer. Hvis ikke der er en eneste værdi indskrevet så er det at højresiden tjekker Fornavn.
                //Hvis bare et felt er udfyldt klarer venstre side af if-statementet den del, og udskriver fejl, fordi den kan se, at en af værdierne passer med databasen, mens alle andre værdier ikke gør.
                if (brugerOplysninger == null || Fornavn == null)
                {
                    nyModel.LoginErrorMessage = "Kunne ikke gennemføre købet grundet forkert Email eller adgangskode og, eller grundet manglende eller forkerte informationer";
                    return View("Checkoutdata", nyModel);
                }
                //Er begge sider af if-statementet false betyder det at alle data er indskrevet og at de er korrekte ift. databasen og derfor returneres viewet "KøbGennemført".
                else
                {
                    return View("KøbGennemført");
                }
            }
        }

        //Denne post bruges under checkoutsiden hvor dataene for brugeren ikke er indskrevet på forhånd, hvilket betyder at brugeren ikke er logget ind.
        //Den fungerer ved at tjekke om der er nogle af felterne som er tomme. Hvis bare ét af felterne ikke er udfyldt så giver den udslag og skriver en error ud.
        [HttpPost]
        //Vi angiver, at vi vil bruge "Bruger" tabellen fra vores database, og vi medgiver en parameter "nyModel", som vi kan bruge til, at tilgå vores get og set metoder fra Bruger modellen.
        public ActionResult Checkoutnodata(Bruger nyModel)
        {
            //Vi bruger vores connectionstring til at lave et nyt obejt "db" så vi kan tilgå vores database.
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //Vi starter med at lave en string for hver værdi der skal udfyldes og sætter den string lig med det som står udfyldt i formen af brugeren.
                string Fornavn = nyModel.ForNavn;
                string EfterNavn = nyModel.EfterNavn;
                string Adresse = nyModel.Adresse;
                string Postnr = nyModel.Postnr;
                string By = nyModel.By;
                string Telefon = nyModel.Telefon;
                string Land = nyModel.Land;
                string Mail = nyModel.Mail;
                string Adgangskode = nyModel.Adgangskode;

                //Med if-statementet her kan vi altså tjekke om der er nogen felter der mangler at blive udfyldt. 
                //Ved at bruge || som det boolske udtryk, så tjekker if-statementet ikke de næste boolske udtryk efter den som har givet udslag.
                //Den tjekker altså slavisk om der mangler information i hvert af fleterne, hvis ja, så udskrives en errormessage og viewet returneres samt modellen. 
                if (Fornavn == null || EfterNavn == null || Adresse == null || Postnr == null || By == null || Telefon == null || Land == null || Mail == null)
                {
                    nyModel.LoginErrorMessage = "Kunne ikke gennemføre købet grundet manglende informationer";
                    return View("Checkoutnodata", nyModel);
                }
                //Hvis den derimod ikke giver udslag og alle er false, betyder det at alle værdier er udfyldt og viewet "KøbGennemført" returneres.
                else
                {
                    return View("KøbGennemført");

                }
            }
        }

        //Her returneres KøbGennemført viewet.
        public ActionResult KøbGennemført()
        {
            return View();
        }
    }
}