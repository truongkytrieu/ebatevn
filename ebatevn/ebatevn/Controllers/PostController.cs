﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ebatevn.Data;
using ebatevn.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ebatevn.Controllers
{
    public class PostController : Controller
    {
        MongoDBContext dbContext = new MongoDBContext();

        private readonly IDistributedCache _cache;

        public PostController(IDistributedCache cache)
        {
            _cache = cache;
        }

        // GET: Post
        public ActionResult Index()
        {
            List<Post> postList = dbContext.Posts.Find(m => true).ToList();
            return View(postList);
        }

        // GET: Post/Details/5
        public ActionResult Details(string id)
        {
            var entity = dbContext.Posts.Find(m => m.Id == id).FirstOrDefault();
            return View(entity);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post entity)
        {
            try
            {
                // TODO: Add insert logic here
                //entity.Id = Guid.NewGuid();
                dbContext.Posts.InsertOne(entity);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Edit/5
        public ActionResult Edit(string id)
        {
            var entity = dbContext.Posts.Find(m => m.Id ==id).FirstOrDefault();
            return View(entity);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post entity)
        {
            try
            {
                // TODO: Add update logic here
                // You can use the UpdateOne to get higher performance if you need.
                dbContext.Posts.ReplaceOne(m => m.Id == entity.Id, entity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        public ActionResult Delete(string id)
        {
            return View(dbContext.Posts.Find(m=>m.Id.Equals(id)).FirstOrDefault());
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Post entity)
        {
            try
            {
                // TODO: Add delete logic here
                dbContext.Posts.DeleteOne(m => m.Id == id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #region IMPLEMENT REDIS CACHE
        [HttpGet]
        public async Task<string> Get()
        {
            var cacheKey = "TheTime";
            var existingTime = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return "Fetched from cache : " + existingTime;
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                _cache.SetString(cacheKey, existingTime);
                return "Added to cache : " + existingTime;
            }
        }


        [HttpGet]
        public async Task<string> GetList()
        {
            var cacheKey = "Setting";
            var values = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(values))
            {
                return values;
            }
            else
            {
                var itemsFromjSON = dbContext.Posts.Find(m => true).ToList();
                values = JsonConvert.SerializeObject(itemsFromjSON);
                _cache.SetString(cacheKey, values);
                return "Added to cache : " + values;
            }

            //var itemsFromjSON = JsonConvert.DeserializeObject<IEnumerable<TwitterItem>>(jSONText);
        }
        #endregion
    }
}