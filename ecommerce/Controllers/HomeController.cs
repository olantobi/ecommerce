using ecommerce.Contracts.Repositories;
using ecommerce.DAL.Data;
using ecommerce.DAL.Repository;
using ecommerce.Model;
using ecommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        IRepositoryBase<Customer> customers;
        IRepositoryBase<Product> products;
        IRepositoryBase<Basket> baskets;
        BasketService basketService = null;

        public HomeController(IRepositoryBase<Customer> customers, IRepositoryBase<Product> products, IRepositoryBase<Basket> baskets)
        {
            this.customers = customers;
            this.products = products;
            this.baskets = baskets;
            this.basketService = new BasketService(this.baskets);
        }


        public ActionResult Index()
        {
            var productList = products.GetAll();

            return View(productList);
        }

        public ActionResult Details(int id)
        {
            Product product = products.GetById(id);

            return View(product);
        }

        public ActionResult BasketSummary()
        {
            var model = basketService.GetBasket(this.HttpContext);

            return View(model.BasketItems);
        }

        public ActionResult AddToBasket(int id)
        {
            basketService.AddToBasket(this.HttpContext, id, 1);

            return RedirectToAction("BasketSummary");
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View((User as ClaimsPrincipal).Claims);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}