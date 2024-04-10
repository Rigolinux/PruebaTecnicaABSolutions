using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;

namespace PruebaTecnicaABSolutions.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserServices userServices;


        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if(!int.TryParse(businees, out int id)) { }

                
            if (role == "1")
            {
                var users = await userServices.GetAllUsers();
                return View(users);
                
            }
            if (role == "2")
            {
                var users = await userServices.GetAllUsers(id);
                return View(users);

            }

            return RedirectToAction("Index", "Home");

        }
        
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create()
        {
            UserViewCreation userViewCreation = new UserViewCreation();
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
             var businees = data[3].Value;
            
            if (role == "1")
            {
                userViewCreation.businessViews = await userServices.GetViewBusinesList();
                userViewCreation.userTypes = await userServices.GetALlListUserTypes();
            }
            if(role == "2")
            {
                userViewCreation.userTypes = await userServices.GetListUserTypes();
            }
            
            return View(userViewCreation);
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create(UserViewCreation userViewCreation)
        {
            if (ModelState.IsValid)
            {
                var data = HttpContext.User.Claims.ToList();
                var role = data[2].Value;
                var businees = data[3].Value;

                // Todo : validate Integrity of data 
                if (role == "1")
                {

                    await userServices.CreateUser(userViewCreation);

                }
                if (role == "2")
                {
                    if (!int.TryParse(businees, out int id)) { }
                        
                        
                    userViewCreation.BusinessId = id;

                    await userServices.CreateUser(userViewCreation);
                }
                return RedirectToAction("Index");
            }
                return RedirectToAction("Create");


        }



        [Authorize(Roles = "1,2")]
        public  IActionResult  Edit(int id = 0)
        {
            var UserId = id;
            if (UserId == 0)
            {
                return RedirectToAction("Index");
            }



            return RedirectToAction("Index", "Home");
        }
    }
}
