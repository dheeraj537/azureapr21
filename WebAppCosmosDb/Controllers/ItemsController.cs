using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCosmosDb.Models;
using WebAppCosmosDb.Services;

namespace WebAppCosmosDb.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ICosmosDbService cosmosDbService;

        public ItemsController(ICosmosDbService cosmosDbService)
        {
            this.cosmosDbService = cosmosDbService;
        }
        // GET: ItemsController
        public async Task<ActionResult> Index()
        {
            var list = await cosmosDbService.GetItemsAsync("select * from c");

            return View(list);
        }

        // GET: ItemsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ItemsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Item newItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newItem.Id = Guid.NewGuid().ToString();
                    await cosmosDbService.AddItemAsync(newItem);
                    return RedirectToAction(nameof(Index));
                }
                return View(newItem);
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ItemsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Item newItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await cosmosDbService.UpdateItemAsync(newItem.Id, newItem);
                    return RedirectToAction(nameof(Index));
                }
                return View(newItem);
            }

            catch
            {

                return View();
            }
        }
        // GET: ItemsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await cosmosDbService.DeleteItemAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}