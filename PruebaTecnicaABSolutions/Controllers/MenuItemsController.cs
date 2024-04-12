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
                var categoryList = await menuItemsService.MenuCategoryViewList((int)item.BusinessId);

                MenuItemViewUpdate menuCreationUpdate = new MenuItemViewUpdate()
                {
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

            if (role != "1")
            {
                if (id_B != itemViewUpdate.BusinessId)
                {
                    return RedirectToAction("Index");
                }
            }
            await menuItemsService.UpdateMenuItem(itemViewUpdate);
            return RedirectToAction("Index");

        }

        public async Task<IEnumerable<MenuCategoryViewList?>> GetListMenucategory(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }
            IEnumerable<MenuCategoryViewList?> menuCategories;

            if (role == "1")
            {
                menuCategories = await menuItemsService.MenuCategoryViewList(id);

            }
            else
            {
                menuCategories = await menuItemsService.MenuCategoryViewList(id_B);

            }

            return menuCategories;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                bool IsDeleted = await menuItemsService.DeleteMenuItemById(id);
                if (IsDeleted)
                    return Ok();

                return BadRequest();

            }

            bool IsDeletedBu = await menuItemsService.DeleteMenuItemByidandBussines(id, id_B);
            if (IsDeletedBu)
                return Ok();

            return BadRequest();
        }

        public async Task<IActionResult> Create()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            MenuItemViewCreation newMenuCategory = new MenuItemViewCreation()
            {
                BusinessId = id_B,

            };


      
               
                newMenuCategory.menuCategoryViews = await menuItemsService.MenuCategoryViewList(id_B);


            

            return View(newMenuCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItemViewCreation menuItemView)
        {
            var data = HttpContext.User.Claims.ToList();
            
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }
            menuItemView.BusinessId = id_B;
            await menuItemsService.CreateMenuItem(menuItemView);

            return RedirectToAction("Index");

        }

    }
}
