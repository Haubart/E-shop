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
       private DatabaseEntities objDatabaseEntities;
        List<ShoppingCartModel> listOfShoppingCartModels;
        public ShoppingController()
        {
            objDatabaseEntities = new DatabaseEntities();
            listOfShoppingCartModels = new List<ShoppingCartModel>();
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
                                                                         ItemCode = objItem.ItemCode,
                                                                         CateGory = objCate.CategoryName,
                                                                      ItemPrice = objItem.ItemPrice
                                                                     }

                                                                     ).ToList();

            return View(listshoppingViewModels);
        }

        [HttpPost]
        public JsonResult index (string ItemId)
        {
        
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Items objItem = objDatabaseEntities.Items.Single(model => model.ItemID.ToString() == ItemId);
          
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }
            //hvis den allerede findes i vores kurv.
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.Unitprice;

            }
            // hvis det ikke findes i vores kurv
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.Unitprice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(data: new { Succes = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            List<ShoppingCartModel> listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModels);
        }


    }
}