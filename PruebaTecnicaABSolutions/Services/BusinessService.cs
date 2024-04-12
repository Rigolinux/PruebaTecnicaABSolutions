using Microsoft.EntityFrameworkCore;
using PruebaTecnicaABSolutions.Models;

namespace PruebaTecnicaABSolutions.Services
{
    public interface IBusinessService
    {
        Task CreateBusinesses(Business business);
        Task DeleteBusinesses(int id);
        Task<IEnumerable<Business>> GetBusinesses();
        Task<Business?> GetOneBusinesses(int Id);
        Task<bool> UpdateBusinesses(Business business);
    }
    public class BusinessService: IBusinessService
    {
        public async Task<IEnumerable<Business>> GetBusinesses()
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Businesses.ToListAsync();
            }
        }

        public async Task<Business?> GetOneBusinesses(int Id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.Businesses.FirstOrDefaultAsync(e => e.BusinessId == Id);
            }
        }

        public async Task<bool> UpdateBusinesses(Business business)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                Business businessdb = await db.Businesses.FirstOrDefaultAsync(e => e.BusinessId == business.BusinessId);
                if ( businessdb == null)
                {
                    return false;
                }

                businessdb.BusinessName = business.BusinessName;
                businessdb.Address = business.Address;
                business.Phone = business.Phone;
                business.Email = business.Email;
                business.Description = business.Description;
                await db.SaveChangesAsync();
                return true;
            }


        }

        public async Task CreateBusinesses(Business business)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                business.CreationDate = DateTime.Now;
                db.Businesses.Add(business);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteBusinesses(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                Business businessdb = await db.Businesses.FirstOrDefaultAsync(e => e.BusinessId == id);
                if (businessdb == null)
                    return;
                db.Remove(businessdb);
                await db.SaveChangesAsync();
            }
        }
    }
}
