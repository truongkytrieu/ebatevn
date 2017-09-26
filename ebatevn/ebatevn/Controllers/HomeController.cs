using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ebatevn.Models;
using ebatevn.Data;
using MongoDB.Driver;

namespace ebatevn.Controllers
{
    public class HomeController : Controller
    {
        MongoDBContext dbContext = new MongoDBContext();
        public IActionResult Index()
        {
            var setting = dbContext.Settings.Find(m => m.Key.Equals("home.title")).FirstOrDefault();
            ViewData["Title"] = setting.Title;
            return View();
        }

        public IActionResult About()
        {
            var setting = dbContext.Settings.Find(m => m.Key.Equals("about.title")).FirstOrDefault();
            ViewData["Title"] = setting.Title;
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
