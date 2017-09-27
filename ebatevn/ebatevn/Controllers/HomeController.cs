using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ebatevn.Models;
using ebatevn.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ebatevn.Helpers;
using System.Collections.Generic;

namespace ebatevn.Controllers
{
    public class HomeController : Controller
    {
        MongoDBContext dbContext = new MongoDBContext();
        private readonly IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            // Implement cache
            // Set cache Settings
            var cacheSettingKey = Constants.Caches.Settings;
            var cacheSettingValues = _cache.GetString(cacheSettingKey);
            if (string.IsNullOrEmpty(cacheSettingValues))
            {
                var itemsFromjSON = dbContext.Settings.Find(m => true).ToList();
                cacheSettingValues = JsonConvert.SerializeObject(itemsFromjSON);
                _cache.SetString(cacheSettingKey, cacheSettingValues);
            }
            
            // For faster set cache title home page in caches Settings
            var cacheKeyHomeTitle = Constants.Caches.HomeTitle;
            var cacheHomeTitleValues = _cache.GetString(cacheKeyHomeTitle);
            if (string.IsNullOrEmpty(cacheHomeTitleValues))
            {
                var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(cacheSettingValues);
                var home = settings.Where(m => m.Key.Equals("home.title")).FirstOrDefault();
                cacheHomeTitleValues = home.Title;
                _cache.SetString(cacheKeyHomeTitle, cacheHomeTitleValues);
            }
            
            ViewData["Title"] = cacheHomeTitleValues;
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
