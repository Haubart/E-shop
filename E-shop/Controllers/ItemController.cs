using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models;
using E_shop.ViewModel;

namespace E_shop.Controllers
{
    public class ItemController : Controller
    {
        // Vi laver et objekt af databasen så vi kan tilgå den alle steder gennem controlleren
        private DatabaseEntities objDatabaseEntities;

        public ItemController()
        {
            objDatabaseEntities = new DatabaseEntities();
        }
        

        // Vi laver et Addimage view som vi skal tilføje produkter til vores hjemmeside:
        public ActionResult Addimage()
        {


            // vi laver et objItemViewModel af ItemViewModel og tilføjer de forskellige kategorier til vores objItemViewModel,
            // så vi kan vælge kategorier til vores produkter:
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItems = (from objCat in objDatabaseEntities.Categories
                                                        select new SelectListItem()
                                                        {
                                                            Text = objCat.CategoryName,
                                                            Value = objCat.CategoryId.ToString(),
                                                            Selected = true
                                                        });
            return View(objItemViewModel);
        }

        // Her til går vi vores JsonResult som modtager objItemViewModel som laver vores produkt og tilføjer den til 
        // vores database
        [HttpPost]
        public JsonResult Addimage(ItemViewModel objItemViewModel)
        {
            // her laver vi en string som holder vores path til billedet lokalt set
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));
            // vi tiføjer vores relevante objekter til  
            Items objItem = new Items();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryID = objItemViewModel.CategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemID = Guid.NewGuid();
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemPrice = objItemViewModel.ItemPrice;
            objDatabaseEntities.Items.Add(objItem);
            objDatabaseEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
    
        }

    }
}