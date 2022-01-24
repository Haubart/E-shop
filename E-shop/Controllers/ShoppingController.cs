using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models;
using E_shop.ViewModel;

namespace E_shop.Controllers
{
    public class ShoppingController : Controller
    {
        DatabaseEntities objDatabaseEntities;

        public ShoppingController()
        {
            objDatabaseEntities = new DatabaseEntities();
        }
        public ActionResult index()
        {
            IEnumerable<ShoppingViewModel> listshoppingViewModels = (from objItem in objDatabaseEntities.Items
                                                                     join
                                                                        objCate in objDatabaseEntities.Categories
                                                                        on objItem.CategoryID equals objCate.CategoryId
                                                                     select new ShoppingViewModel()
                                                                     {
                                                                         ImagePath = objItem.ImagePath,
                                                                         ItemName = objItem.ItemName,
                                                                         Description = objItem.Description,
                                                                         ItemId = objItem.ItemID,
                                                                         CateGory = objCate.CategoryName
                                                                     }

                                                                     ).ToList();

            return View();
        }
    }
}