using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Data;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    //localhost:5001/home
    public class HomeController:Controller
    {
        //localhost:5001/home/index
        public IActionResult Index()
        {
            var productViewModel = new ProductViewModel(){
                Products = ProductRepository.Products
            };

            return View(productViewModel);
        }
        //localhost:5001/home/about
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact(){
            return View("MyView");
        }
    }
}