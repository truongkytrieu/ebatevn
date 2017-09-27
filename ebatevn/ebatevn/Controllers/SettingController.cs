using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ebatevn.Data;
using ebatevn.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Caching.Distributed;
using ebatevn.Helpers;

namespace ebatevn.Controllers
{
    public class SettingController : Controller
    {
        MongoDBContext dbContext = new MongoDBContext();

        private readonly IDistributedCache _cache;

        public SettingController(IDistributedCache cache)
        {
            _cache = cache;
        }

        // GET: Setting
        public ActionResult Index()
        {
            List<Setting> SettingList = dbContext.Settings.Find(m => true).ToList();
            return View(SettingList);
        }

        // GET: Setting/Details/5
        public ActionResult Details(string id)
        {
            var entity = dbContext.Settings.Find(m => m.Id == id).FirstOrDefault();
            return View(entity);
        }

        // GET: Setting/Create
        public ActionResult Create()
        {
            return View();
        }

        // Setting: Setting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Setting entity)
        {
            try
            {
                // TODO: Add insert logic here
                //entity.Id = Guid.NewGuid();
                dbContext.Settings.InsertOne(entity);
                // Clear cached
                var cacheSettingKey = Constants.Caches.Settings;
                _cache.Remove(cacheSettingKey);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Setting/Edit/5
        public ActionResult Edit(string id)
        {
            var entity = dbContext.Settings.Find(m => m.Id ==id).FirstOrDefault();
            return View(entity);
        }

        // Setting: Setting/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Setting entity)
        {
            try
            {
                // TODO: Add update logic here
                // You can use the UpdateOne to get higher performance if you need.
                dbContext.Settings.ReplaceOne(m => m.Id == entity.Id, entity);
                var cacheSettingKey = Constants.Caches.Settings;
                _cache.Remove(cacheSettingKey);
                if (entity.Key == "home.title")
                {
                    var cacheKeyHomeTitle = Constants.Caches.HomeTitle;
                    _cache.Remove(cacheKeyHomeTitle);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Setting/Delete/5
        public ActionResult Delete(string id)
        {
            return View(dbContext.Settings.Find(m=>m.Id.Equals(id)).FirstOrDefault());
        }

        // Setting: Setting/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Setting entity)
        {
            try
            {
                // TODO: Add delete logic here
                dbContext.Settings.DeleteOne(m => m.Id == id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}