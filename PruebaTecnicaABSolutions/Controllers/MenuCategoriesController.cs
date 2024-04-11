using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;
using Microsoft.AspNetCore.Authorization;

namespace PruebaTecnicaABSolutions.Controllers
{
    [Authorize]

    public class MenuCategoriesController : Controller
    {
        
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
               

                return View(menuCategoryView);

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

           
            if(idBusinees != menuCategory.BusinessId)
            {
                return RedirectToAction("Index");
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

    
    }
}
