using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;
namespace PruebaTecnicaABSolutions.Controllers
{
    public class AccessController : Controller
    {
        private readonly IEncriptService encriptService;
        private readonly IUserServices userServices;
        private readonly IBusinessService businessService;

        public AccessController(IEncriptService encriptService, IUserServices userServices, IBusinessService businessService
            
        )
        {
            this.encriptService = encriptService;
            this.userServices = userServices;
            this.businessService = businessService;
        }

        public ActionResult Login()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            
            if(claimsUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel login)
        {
            if(login.Email != null)
            {
               var user = await this.userServices.FindUser(login.Email);
                
                if (user != null && user.Password != null)
                {
                    bool isValid = this.encriptService.ValidatePassword(login.Password, user.Password);
                    
                    if(isValid )
                    {
                        if(user.BusinessId != null)
                            user.Business = await businessService.GetOneBusinesses((int)user.BusinessId);
                        if (user.UserTypeId != null)
                            user.UserType = await userServices.GetUserType((int)user.UserTypeId);
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim(ClaimTypes.Role, user.UserTypeId.ToString()),
                            new Claim("Bussiness", user.BusinessId.ToString()),           
                            new Claim("BussinessName",user.Business.BusinessName.ToString()),
                            new Claim("RoleName",user.UserType.TypeName.ToString())

                        };
                        // use when the user is 3
                       // foreach (string rol in usuario.Roles)
                       // {
                       //     claims.Add(new Claim(ClaimTypes.Role, rol));
                       // }
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = login.KeepLogin
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            properties);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewData["ValidateMessage"] = "Contraseña o Correo Invalidos Intente de nuevo";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }

        

    }
}
