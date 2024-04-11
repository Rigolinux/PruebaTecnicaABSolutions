using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;
using Syncfusion.EJ2.Layouts;

namespace PruebaTecnicaABSolutions.Controllers
{
    [Authorize]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemsService menuItemsService;
        private readonly IUserServices userServices;

        public MenuItemsController(IMenuItemsService menuItemsService, IUserServices userServices)
        {
            this.menuItemsService = menuItemsService;
            this.userServices = userServices;
        }

        // get: menuitems
        public async Task<IActionResult> Index()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id)) { }

            if (role == "1")
            {
                var items = await menuItemsService.FindAllMenuItems();
                return View(items);
                


            }
            var itemsBusniees = await menuItemsService.FindAllMenuItemsByBusiness(id);
            return View(itemsBusniees);
        }




        public async Task<IActionResult> Details(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                var item = await menuItemsService.FindOneItemByid(id);
                if (item == null)
                {
                    return RedirectToAction("Index");
                }
                
                return View(item);
            }
            var itemB = await menuItemsService.FindOneItemByidandBussines(id, id_B);
            if (itemB == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(itemB);
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                
                var item = await menuItemsService.FindOneItemByid(id);
               // var bussnessList = await userServices.GetViewBusinesList();

                if (item == null)
                {
                    return RedirectToAction("Index");
                }
               var categoryList = await menuItemsService.MenuCategoryViewList(item.BusinessId);

                MenuItemViewUpdate menuCreationUpdate = new MenuItemViewUpdate() {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Description = item.Description,
                    Price = item.Price,
                    BusinessId = item.BusinessId,
                    CategoryId = item.CategoryId,
                    menuCategoryViews = categoryList
                }; 
                return View(menuCreationUpdate);
            }
            var itemB = await menuItemsService.FindOneItemByidandBussines(id, id_B);
       
            if (itemB == null)
            {
                return RedirectToAction("Index");
            }
            var categoryListB = await menuItemsService.MenuCategoryViewList(id_B);
            MenuItemViewUpdate menuItemView = new MenuItemViewUpdate()
            {
                ItemId = itemB.ItemId,
                ItemName = itemB.ItemName,
                Description = itemB.Description,
                Price = itemB.Price,
                BusinessId = itemB.BusinessId,
                CategoryId = itemB.CategoryId,
                menuCategoryViews = categoryListB
            };
            return View(menuItemView);

        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(MenuItemViewUpdate itemViewUpdate)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if(role != "1")
            {
                if(id_B != itemViewUpdate.BusinessId)
                {
                    return RedirectToAction("Index");
                }
            }
            await menuItemsService.UpdateMenuItem(itemViewUpdate);
            return RedirectToAction("Index");
            
        }

        public async Task<IEnumerable<MenuCategoryViewList?>> GeListMenucategory(int id) {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                return await menuItemsService.MenuCategoryViewList(id);

            }
            else
            {
                return await menuItemsService.MenuCategoryViewList(id_B);

            }
        }
            //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId");
            //    ViewData["CategoryId"] = new SelectList(_context.MenuCategories, "CategoryId", "CategoryId");
            //    return View();
            //}

            //// POST: MenuItems/Create
            //// To protect from overposting attacks, enable the specific properties you want to bind to.
            //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Create([Bind("ItemId,ItemName,Description,CategoryId,Price,BusinessId,AddedDate")] MenuItem menuItem)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        _context.Add(menuItem);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuItem.BusinessId);
            //    ViewData["CategoryId"] = new SelectList(_context.MenuCategories, "CategoryId", "CategoryId", menuItem.CategoryId);
            //    return View(menuItem);
            //}

            //// GET: MenuItems/Edit/5
            //public async Task<IActionResult> Edit(int? id)
            //{
            //    if (id == null || _context.MenuItems == null)
            //    {
            //        return NotFound();
            //    }

            //    var menuItem = await _context.MenuItems.FindAsync(id);
            //    if (menuItem == null)
            //    {
            //        return NotFound();
            //    }
            //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuItem.BusinessId);
            //    ViewData["CategoryId"] = new SelectList(_context.MenuCategories, "CategoryId", "CategoryId", menuItem.CategoryId);
            //    return View(menuItem);
            //}

            //// POST: MenuItems/Edit/5
            //// To protect from overposting attacks, enable the specific properties you want to bind to.
            //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,Description,CategoryId,Price,BusinessId,AddedDate")] MenuItem menuItem)
            //{
            //    if (id != menuItem.ItemId)
            //    {
            //        return NotFound();
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            _context.Update(menuItem);
            //            await _context.SaveChangesAsync();
            //        }
            //        catch (DbUpdateConcurrencyException)
            //        {
            //            if (!MenuItemExists(menuItem.ItemId))
            //            {
            //                return NotFound();
            //            }
            //            else
            //            {
            //                throw;
            //            }
            //        }
            //        return RedirectToAction(nameof(Index));
            //    }
            //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuItem.BusinessId);
            //    ViewData["CategoryId"] = new SelectList(_context.MenuCategories, "CategoryId", "CategoryId", menuItem.CategoryId);
            //    return View(menuItem);
            //}

            //// GET: MenuItems/Delete/5
            //public async Task<IActionResult> Delete(int? id)
            //{
            //    if (id == null || _context.MenuItems == null)
            //    {
            //        return NotFound();
            //    }

            //    var menuItem = await _context.MenuItems
            //        .Include(m => m.Business)
            //        .Include(m => m.Category)
            //        .FirstOrDefaultAsync(m => m.ItemId == id);
            //    if (menuItem == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(menuItem);
            //}

            //// POST: MenuItems/Delete/5
            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> DeleteConfirmed(int id)
            //{
            //    if (_context.MenuItems == null)
            //    {
            //        return Problem("Entity set 'ABPruebaTecnicaContext.MenuItems'  is null.");
            //    }
            //    var menuItem = await _context.MenuItems.FindAsync(id);
            //    if (menuItem != null)
            //    {
            //        _context.MenuItems.Remove(menuItem);
            //    }

            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            //private bool MenuItemExists(int id)
            //{
            //  return (_context.MenuItems?.Any(e => e.ItemId == id)).GetValueOrDefault();
            //}
        }
}
