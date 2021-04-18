using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shopapp.webui.Data;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class ProductController:Controller
    {
        public IActionResult Index()
        {
            //var product = new Product{Name="Iphone X", Price=6000,Description="Güzel telefon"};

            //ViewData["Category"]="Telefonlar";
            //ViewData["Product"]=product;

            //ViewBag.Category = "Telefonlar";
            //ViewBag.Product = product;

            return View();
        }
        //localhost:5000/product/list
        public IActionResult List(int? id,string q)
        {
            //Console.WriteLine(q);
            //Console.WriteLine(HttpContext.Request.Query["q"].ToString());
            var products = ProductRepository.Products;
            if(id != null){
                products = products.Where(p=>p.CategoryId==id).ToList();
            }
            if(!string.IsNullOrEmpty(q)){
                products = products.Where(i=>i.Name.Contains(q) || i.Description.Contains(q)).ToList();
            }

            var productViewModel = new ProductViewModel(){
                Products = products
            };

            return View(productViewModel);
        }
        [HttpGet]
        //localhost:5000/product/details
        public IActionResult Details(int id)
        {
            return View(ProductRepository.GetProductById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product p)
        {
            
            if (ModelState.IsValid)
            {
                ProductRepository.AddProduct(p);
                return RedirectToAction("list");
            }
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(p);
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(ProductRepository.GetProductById(id));
        }
        [HttpPost]
        public IActionResult Edit(Product p)
        {
            ProductRepository.EditProduct(p);
            return RedirectToAction("list");
        }
        [HttpPost]
        public IActionResult Delete(int ProductId)
        {
            ProductRepository.DeleteProduct(ProductId);
            return RedirectToAction("list");
        }
    }
}