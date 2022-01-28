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

        // Her laver der en række om objekter som skal bruges igennem controlleren,
        // så de kan tilgåes alle steder igennem controlleren:
        private DatabaseEntities objDatabaseEntities;
        List<ShoppingCartModel> listOfShoppingCartModels;
        private IEnumerable<ShoppingViewModel> listShoppingViewModel;
        public IEnumerable<ShoppingViewModel> return_Model;


    
        public ShoppingController()
        {
            // Her laves en DatabaseEntities
            objDatabaseEntities = new DatabaseEntities();
            // Der lave en liste af denne shoppingCartmodel,som indeholder alle værider fra ShoppingCartModel,
           // altså alle get og set metoderne for de foreskellige værdier:
            listOfShoppingCartModels = new List<ShoppingCartModel>();
        }

        // Her har vi vores main vindue altså index vinduet, i dette "view" sendes der
        // sendes der to værdier tilbage til. Dette er vores filter og searching funktion bestemmer,
        // som bestemmer hvilke "Produkter" der skal vises på siden!
        public ActionResult index(string searching, string filter)
            {

            // Her tjekker vi efter om searching er lig med null og laver searching værdien om til 
            // blank, da programmet ikke kan arbejde med en værdi hvis searching == null;
            if (searching == null)
            {
                searching = "";
            }
            else
            {

            }

            // Det er her vi laver vores objekter som bliver displayet i vores index:

            //Dette laver vi igennem vores listShoppingViewModel som vi giver en række variable, som vi 
            // så kan fremvise på index siden, værdierne får vi gennem objItem fra Items databasen, som holder 
            // alle vores produkter, vi tilføjer desuden også en anden ting til listshopping, som dog ikke bliver
            // vist. Dette er objCate, som holder vores kategorier til de forskellige produkter. Dette tilader også 
            // os at skabe denne filter funktion:
                                       listShoppingViewModel = (from objItem in objDatabaseEntities.Items
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
            //her begynder vi at officelt at lave filter og søge funktionen:
            // Vi tjekker her om vores seaching er ligmed blank og om filter er lig med null
            // så skal vi bare returnere selve modellen uden noget andet.
            if (searching == "" && filter == null)
            {
                return_Model = listShoppingViewModel;
            }
            // hvis 'filter' ikke var tomt, kommer vi til dette statment, hvilket betyder vi har lagt 
            // et filter på, så vi kun får vist de produkter som har denne kategori i sig;
            else if ( searching == "")
            {
                listShoppingViewModel = (from objItem in objDatabaseEntities.Items
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

                // vi kigger altså i modellen om 'Category' indeholder vores filter så skal den tilføje det til 
                // listShoppingViewModel, som vi som vi så sætter til vores return_Model som gør at vi får sendt denne model
                // med og ikke den der blevet skabt før!
                return_Model = listShoppingViewModel.Where(x => x.CateGory.Contains(filter));
            }

            // hvis filter var tomt kommer vi til dette statement:
            // Her ligger vi et søge ord ind og kigger efter om vores model har et tegn eller et ord som minder om 
            // søge ordet.
            else if (filter == null )
            {
                listShoppingViewModel = (from objItem in objDatabaseEntities.Items
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

                // vi kigger altså i modellen om 'Category' indeholder vores filter så skal den tilføje det til 
                // listShoppingViewModel, som vi som vi så sætter til vores return_Model som gør at vi får sendt denne model
                // med og ikke den der blevet skabt før!
                return_Model = listShoppingViewModel.Where(x => x.ItemName.Contains(searching));
            }
            return View(return_Model);


         

        }
        
        // Her laver vi et JsonResult som modtager et ItemId fra vores Index HTML, som vi skal bruge til at tilføje vores 
        // 'Produkt' til  kurven
        [HttpPost]
        public JsonResult index (string ItemId)
        {
            //Her laver vi et objShoppingCartModel 
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            //Her laver vi et objItem, som får tildelt det produkt som vores JsonResult modtager:
            Items objItem = objDatabaseEntities.Items.Single(model => model.ItemID.ToString() == ItemId);

            // Hertjekkes der om vores CartCounter != null altså ikke lig med hinanden
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }
            //hvis den allerede findes i vores kurv udfører vi dette statement
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                //Her tilføjer vi produktet som allerede findes i modellen engang til til vores objShoppingCartModel
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                // tilføjer Quantity med 1, da vi tilføjer 1 mere til Quantity
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                // Vi gør sådan vores total er lig med antalet af produkter er ganget med styk prisen:
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.Unitprice;
             
            }
            // hvis det ikke findes i vores kurv, sker dette statement:
            else
            {
                // Her tilføjer vi bare alle relevante objekter til objShoppingCartModel så vi kan vise det i 
                // '  public ActionResult ShoppingCart()'
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.Unitprice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }
            //vi sætter CartCounter til at være antalet af counts i listOfShoppingCartModels til at være = Session["CartCounter"]
            Session["CartCounter"] = listOfShoppingCartModels.Count;
            // Vi sætter CartItem = listOfShoppingCartModels
            Session["CartItem"] = listOfShoppingCartModels;
            //Vi retunere counten og vores succes er true
            return Json(data: new { Succes = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            //Her tager vi vores listOfShoppingCartModels ind i shoppingCart 
            List<ShoppingCartModel> listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            // vi sender listOfShoppingCartModels til viewet:
            return View(listOfShoppingCartModels);
        }


        [HttpPost]
        public ActionResult AddOrder()
        {
            

            // Herr tager vi vores listOfShoppingCartModels ind i programmet 
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            // der laves et objekt af Order
            Order objOrder = new Order()
            {
                //Vi tilføjer datoen 
                OrderDate = DateTime.Now,
                // vi laver et number som har en special kode til vores OrderNumber
                OrderNumber = string.Format("{0:ddmmyyHHmmsss}", DateTime.Now),
            };
            // Vi tilføjer objketet til databasen:
            objDatabaseEntities.Order.Add(objOrder);
            // gemmer det
            objDatabaseEntities.SaveChanges();
            // vi laver et OrderId, så vi kan lave 
          

            foreach( var item in listOfShoppingCartModels)
            {
                // vi laver et orderDetail for hvert produkt der er i
                // listOfShoppingCartModels:
                OrderDetails objOrderDetails = new OrderDetails();
                objOrderDetails.Total = item.Total;
                objOrderDetails.ItemId = item.ItemId;
                objOrderDetails.OrderId = objOrder.OrderId;
                objOrderDetails.Quantity = item.Quantity;
                objOrderDetails.UnitPrice = item.Unitprice;
                //tilføjer objOrderDetails til databasen
                objDatabaseEntities.OrderDetails.Add(objOrderDetails);
                //gemmer
                objDatabaseEntities.SaveChanges();

            }
            // sætter  Session["CartItem"] til null:
            Session["CartItem"] = null;
            // sætter   Session["CartCounter"] til null:
            Session["CartCounter"] = null;
            // Når denne ActionResult aktiveres så går vi til denne adreese /Home/Checkout (mere om den i Home controlleren):
            return RedirectToAction("Checkout", "Home");
        }


    }
}