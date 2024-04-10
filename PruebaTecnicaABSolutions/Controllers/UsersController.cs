using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;

namespace PruebaTecnicaABSolutions.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserServices userServices;

        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userServices.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            UserViewCreation userViewCreation = new UserViewCreation();
            userViewCreation.businessViews = await userServices.GetViewBusinesList();
            return View(userViewCreation);
        }

        [HttpPost]
        public  IActionResult Create(UserViewCreation userViewCreation)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
    }
}
