using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using RavenMVCProducts.Extensions;
using RavenMVCProducts.Models;
using RavenMVCProducts.ViewModels;

namespace RavenMVCProducts.Controllers
{
    public class ProductController : RavenController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            var model = _session.Query<Product>()
               .Paging(CurrentPage, DefaultPage, PageSize)
               .ToList();

            Mapper.Map<List<Product>, List<ProductViewModel>>(model);

            return View(model);
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(string id)
        {
            var model = _session.Load<Product>(id);
            return View(model);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            var model = new Product();
            return View(model);
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                _session.Store(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(string id)
        {
            var model = _session.Load<Product>(id);
            return View(model);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                _session.Store(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(string id)
        {
            var model = _session.Load<Product>(id);
            return View(model);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                _session.Advanced.DatabaseCommands.Delete(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult StoreSomeProductInDatabase()
        {
            var product = new Product
                              {
                                  Name = "Product Name",
                                  CategoryId = "category/1024",
                                  SupplierId = "supplier/16",
                                  Code = "H11050",
                                  StandardCost = 250,
                                  ListPrice = 189
                              };

            _session.Store(product);
            _session.SaveChanges();

            //return Content(product.Id);
            return RedirectToAction("Index");
        }

        public ActionResult InsertSomeMoreProducts()
        {
            for (int i = 0; i < 50; i++)
            {
                var product = new Product
                                  {
                                      Name = "Product Name " + i,
                                      CategoryId = "category/1024",
                                      SupplierId = "supplier/16",
                                      Code = "H11050" + i,
                                      StandardCost = 250 + (i*10),
                                      ListPrice = 189 + (i*10),
                                  };
                _session.Store(product);
            }

            _session.SaveChanges();

            //return Content("Products successfully created");
            return RedirectToAction("Index");
        }

        public ActionResult GetProduct(int id)
        {
            Product product = _session.Load<Product>(id);
            return Content(product.Name);
        }

        public ActionResult LoadAndUpdateProduct()
        {
            Product product = _session.Load<Product>("products/5");
            product.ListPrice -= 10;
            _session.SaveChanges();
            return Content("Product 5 successfully updated");
        }

        public ActionResult DeleteProduct(int id)
        {
            Product product = _session.Load<Product>(id);
            if (product == null)
                return HttpNotFound("Product {0} does not exist");
            _session.Delete(product);
            _session.SaveChanges();
            return Content(string.Format("Product {0} successfully deleted", id));
        }

        /// <summary>
        /// Get all the products that are available for sale (discontinued equal to false) ordered by the product’s list price
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDiscontinuedProducts()
        {
            var products = from product in _session.Query<Product>()
                           where product.Discontinued == false
                           orderby product.ListPrice
                           select product;

            return View(products.ToList());
        }
    }
}