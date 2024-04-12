using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PruebaTecnicaABSolutions.Models;
using System.Configuration;
using PruebaTecnicaABSolutions.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace PruebaTecnicaABSolutions.Services
{
    public interface IUserServices
    {
        Task CreateUser(UserViewCreation user);
        Task<bool> DeleteUser(int id);
        Task<User?> FindUser(string mail);
        Task<User?> FindUserbyId(int id);
        Task<IEnumerable<UserTypesViewList>> GetALlListUserTypes();
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<IEnumerable<UserViewModel>> GetAllUsers(int businessId);
        Task<IEnumerable<UserTypesViewList>> GetListUserTypes();
        Task<UserType?> GetUserType(int id);
        Task<IEnumerable<BusinessViewList>> GetViewBusinesList();
        Task<bool> UpdateUser(UserViewUpdate user);
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
                    .Join(db.Businesses,
                            user => user.BusinessId,
                            business => business.BusinessId,
                            (user, business) => new { User = user, Business = business })
                    .Join(db.UserTypes,
                            joined => joined.User.UserTypeId,
                            userType => userType.UserTypeId,
                            (joined, userType) => new UserViewModel
                            {
                                FirstName = joined.User.FirstName,
                                LastName = joined.User.LastName,
                                Email = joined.User.Email,
                                Business = joined.Business.BusinessName,
                                BusinessId = joined.User.BusinessId,
                                UserId = joined.User.UserId,
                                UserType =userType.TypeName
                            })
                    .OrderByDescending(e => e.UserId)
                    .ToListAsync();
            }
        }
        public async Task<IEnumerable<UserViewModel>> GetAllUsers(int idBussines)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Users
                     .Where(e => e.BusinessId == idBussines && e.UserTypeId != 1)
                    .Join(db.Businesses,
                            user => user.BusinessId,
                            business => business.BusinessId,
                            (user, business) => new { User = user, Business = business })
                    .Join(db.UserTypes,
                            joined => joined.User.UserTypeId,
                            userType => userType.UserTypeId,
                            (joined, userType) => new UserViewModel
                            {
                                FirstName = joined.User.FirstName,
                                LastName = joined.User.LastName,
                                Email = joined.User.Email,
                                Business = joined.Business.BusinessName,
                                BusinessId = joined.User.BusinessId,
                                UserId = joined.User.UserId,
                                UserType = userType.TypeName
                            })
                    .OrderByDescending(e => e.UserId)
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

        public async Task<User?> FindUser(string mail)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Email == mail);
            }
        }

        public async Task<User?> FindUserbyId(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            }
        }
        public async Task<IEnumerable<UserTypesViewList>> GetALlListUserTypes()
        {
            
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.UserTypes.Select(e => new UserTypesViewList
                {
                    UserTypeId = e.UserTypeId,
                    TypeName = e.TypeName
                })
                  .ToListAsync();
            }
            
        }
        public async Task<IEnumerable<UserTypesViewList>> GetListUserTypes()
        {

            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.UserTypes.Select(e => new UserTypesViewList
                {
                    UserTypeId = e.UserTypeId,
                    TypeName = e.TypeName
                })
                  .Where(e => e.UserTypeId != 1)
                  .ToListAsync();
            }

        }

        public async Task<bool> UpdateUser(UserViewUpdate user)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                User userToUpdate = await db.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                if (userToUpdate == null)
                {
                    return false;
                }
                if (user.Password != null)
                {
                    userToUpdate.Password = encriptService.Encrypt(user.Password);
                }
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Email = user.Email;
                userToUpdate.BusinessId = user.BusinessId;
                userToUpdate.UserTypeId = user.UserType;
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<UserType?> GetUserType(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.UserTypes.FirstOrDefaultAsync(e => e.UserTypeId == id);
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    return false;
                }
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}
