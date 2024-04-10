﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PruebaTecnicaABSolutions.Models;
using System.Configuration;

using Microsoft.EntityFrameworkCore;


namespace PruebaTecnicaABSolutions.Services
{
    public interface IUserServices
    {
        Task<IEnumerable<UserViewModel>> GetAllUsers();
    }
   


    public class UserServices : IUserServices
    {

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


    }
}