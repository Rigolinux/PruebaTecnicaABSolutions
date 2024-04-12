using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaABSolutions.Models;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;


namespace PruebaTecnicaABSolutions.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;

            if (role == "1")
                return RedirectToAction("Index", "Businesses");

            return RedirectToAction("Index", "MenuItems");
        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
