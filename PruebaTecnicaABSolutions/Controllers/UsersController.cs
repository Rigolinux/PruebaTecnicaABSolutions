using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;
using Microsoft.AspNetCore.Authorization;

namespace PruebaTecnicaABSolutions.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserServices userServices;


        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [Authorize(Roles ="1")]
        public async Task<IActionResult> Index()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var cookieValue = claim?.Value;// aqui esta el correo
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
        public async Task<IActionResult> Create(UserViewCreation userViewCreation)
        {
            if (ModelState.IsValid)
            {
                userViewCreation.UserType = 1;
                await userServices.CreateUser(userViewCreation);

            }
            return RedirectToAction("Index");

        }
    }
}
