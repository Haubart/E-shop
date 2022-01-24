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
        private DatabaseEntities objDatabaseEntities;

        public ItemController()
        {
            objDatabaseEntities = new DatabaseEntities();
        }
        
        public ActionResult index1()
        {
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


        [HttpPost]
        public JsonResult index1(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

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