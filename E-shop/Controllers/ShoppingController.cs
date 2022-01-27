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

        public ActionResult index(string searching, string button, string CateGory)
        {
            if(searching == null)
            {
                searching = "";
            }
            else
            {

            }
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
             return View(listshoppingViewModels.Where(x => x.ItemName.Contains(searching) || searching == null));
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


        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderId = 0;

            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            Order objOrder = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = string.Format("{0:ddmmyyHHmmsss}", DateTime.Now),
            };
            objDatabaseEntities.Order.Add(objOrder);
            objDatabaseEntities.SaveChanges();
            OrderId = objOrder.OrderId;

            foreach( var item in listOfShoppingCartModels)
            {
                OrderDetails objOrderDetails = new OrderDetails();
                objOrderDetails.Total = item.Total;
                objOrderDetails.ItemId = item.ItemId;
                objOrderDetails.OrderId = OrderId;
                objOrderDetails.Quantity = item.Quantity;
                objOrderDetails.UnitPrice = item.Unitprice;
                objDatabaseEntities.OrderDetails.Add(objOrderDetails);
                objDatabaseEntities.SaveChanges();

            }
            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Checkout", "Home");
        }


    }
}