using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;

namespace PruebaTecnicaABSolutions.Controllers
{
    public class MenuCategoriesController : Controller
    {
        private readonly int _context;
        private readonly IMenuCategoriesService menuCategoriesService;
        private readonly IUserServices userServices;

        public MenuCategoriesController( IMenuCategoriesService menuCategoriesService, IUserServices userServices )
        {

       
            this.menuCategoriesService = menuCategoriesService;
            this.userServices = userServices;
        }

        // GET: MenuCategories
        public async Task<IActionResult> Index()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if(!int.TryParse(businees, out int id)) { }

            if (role == "1")
            {
                var categorys = await menuCategoriesService.FindAllMenuCategory();
                return View(categorys);



            }
            var categorysBusniees = await menuCategoriesService.FindAllMenuCategoryByBusiness(id);
            return View(categorysBusniees);


            
        }
        
        
        public async Task<IActionResult> Edit(int id)
        {



            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int idBusinees)) { }

            if (role == "1")
            {
                var detail = await menuCategoriesService.FindOneCategory(id);
                if (detail == null)
                    return RedirectToAction("Index");
                MenuCategoryViewUpdate menuCategoryView = new MenuCategoryViewUpdate()
                {
                    CategoryId = detail.CategoryId,
                    CategoryName = detail.CategoryName,
                    BusinessId = detail.BusinessId
              
                };
                menuCategoryView.businessViews = await userServices.GetViewBusinesList();

                return View(detail);

            }
            var detailwithb = await menuCategoriesService.FindOneCategory(id, idBusinees);
            if (detailwithb == null)
                return RedirectToAction("Index");

            MenuCategoryViewUpdate menuCategoryViewBusinees = new MenuCategoryViewUpdate()
            {
                CategoryId = detailwithb.CategoryId,
                CategoryName = detailwithb.CategoryName,
                BusinessId = detailwithb.BusinessId

            };
            return View(menuCategoryViewBusinees);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuCategoryViewUpdate menuCategory)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int idBusinees)) { }

            if(role != "1")
            {
                if(idBusinees != menuCategory.BusinessId)
                {
                    return RedirectToAction("Index");
                }
            }

            await menuCategoriesService.UpdateMenuCategory(menuCategory);
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Create()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;

            MenuCategoryViewCreation menu = new MenuCategoryViewCreation();
            
            if (role == "1")
            {
                menu.businessViews = await userServices.GetViewBusinesList();
            }

            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuCategoryViewCreation menuCategory)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int idBusinees)) { }

            if (role != "1")
            {
                menuCategory.BusinessId = idBusinees;
            }

            await menuCategoriesService.CreateMenuCategory(menuCategory);

            return RedirectToAction("Index");
        }

        // GET: MenuCategories/Create
        //public IActionResult Create()
        //{
        //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId");
        //    return View();
        //}

        // POST: MenuCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,BusinessId")] MenuCategory menuCategory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(menuCategory);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuCategory.BusinessId);
        //    return View(menuCategory);
        //}

        // GET: MenuCategories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.MenuCategories == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuCategory = await _context.MenuCategories.FindAsync(id);
        //    if (menuCategory == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuCategory.BusinessId);
        //    return View(menuCategory);
        //}

        // POST: MenuCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,BusinessId")] MenuCategory menuCategory)
        //{
        //    if (id != menuCategory.CategoryId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(menuCategory);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MenuCategoryExists(menuCategory.CategoryId))
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
        //    ViewData["BusinessId"] = new SelectList(_context.Businesses, "BusinessId", "BusinessId", menuCategory.BusinessId);
        //    return View(menuCategory);
        //}

        //// GET: MenuCategories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.MenuCategories == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuCategory = await _context.MenuCategories
        //        .Include(m => m.Business)
        //        .FirstOrDefaultAsync(m => m.CategoryId == id);
        //    if (menuCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuCategory);
        //}

        //// POST: MenuCategories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.MenuCategories == null)
        //    {
        //        return Problem("Entity set 'ABPruebaTecnicaContext.MenuCategories'  is null.");
        //    }
        //    var menuCategory = await _context.MenuCategories.FindAsync(id);
        //    if (menuCategory != null)
        //    {
        //        _context.MenuCategories.Remove(menuCategory);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MenuCategoryExists(int id)
        //{
        //  return (_context.MenuCategories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        //}
    }
}
