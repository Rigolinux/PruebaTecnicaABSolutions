using Microsoft.AspNetCore.Mvc;
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
            var users =  await userServices.GetAllUsers();
            return View(users);
        }
    }
}
