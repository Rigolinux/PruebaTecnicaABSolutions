using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PruebaTecnicaABSolutions.Models;
using System.Configuration;
using PruebaTecnicaABSolutions.Services;
using Microsoft.EntityFrameworkCore;


namespace PruebaTecnicaABSolutions.Services
{
    public interface IUserServices
    {
        Task CreateUser(UserViewCreation user);
        Task<User> FindUser(string mail);
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<IEnumerable<BusinessViewList>> GetViewBusinesList();
    }
   


    public class UserServices : IUserServices
    {
        private readonly IEncriptService encriptService;

        public UserServices(IEncriptService encriptService)
        {
            this.encriptService = encriptService;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Users
                    .Select(e => new UserViewModel
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        Business = e.Business,
                        BusinessId = e.BusinessId,
                        UserId = e.UserId,
                        UserType = e.UserType
                    })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<BusinessViewList>> GetViewBusinesList()
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Businesses
                    .Select(e => new BusinessViewList
                    {
                        BusinessId = e.BusinessId,
                        BusinessName = e.BusinessName
                    })
                    .ToListAsync();
            }

        }
        
        public async Task CreateUser(UserViewCreation user)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                
                User newUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = encriptService.Encrypt(user.Password),
                    BusinessId = user.BusinessId,
                    UserTypeId = user.UserType
                };
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> FindUser(string mail)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Email == mail);
            }
        }
    }
}
