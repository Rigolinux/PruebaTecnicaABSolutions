using PruebaTecnicaABSolutions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PruebaTecnicaABSolutions.Services
{
    public interface IMenuCategoriesService
    {
        Task<bool> CreateMenuCategory(MenuCategoryViewCreation menuCategory);
        Task DeleteMenuCategory(int id);
        Task<IEnumerable<MenuCategory>> FindAllMenuCategory();
        Task<IEnumerable<MenuCategory>> FindAllMenuCategoryByBusiness(int idBussiness);
        Task<MenuCategory?> FindOneCategory(int id, int idBussiness);
        Task<MenuCategory?> FindOneCategory(int id);
        Task<bool> UpdateMenuCategory(MenuCategoryViewUpdate category);
    }
    public class MenuCategoriesService : IMenuCategoriesService
    {  

        public async Task<IEnumerable<MenuCategory>> FindAllMenuCategory()
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.MenuCategories.ToListAsync();
            }
        }

        public async Task<IEnumerable<MenuCategory>> FindAllMenuCategoryByBusiness(int idBussiness)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.MenuCategories.Where(e => e.BusinessId == idBussiness).ToListAsync();
            }
        }

        public async Task<MenuCategory?> FindOneCategory(int id, int idBussiness)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.MenuCategories.FirstOrDefaultAsync(e => e.BusinessId == idBussiness && e.CategoryId == id);
            }

        }
        public async Task<MenuCategory?> FindOneCategory(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.MenuCategories.FirstOrDefaultAsync(e => e.CategoryId == id);
            }

        }


        public async Task<bool> UpdateMenuCategory(MenuCategoryViewUpdate category)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                var categorydb = await db.MenuCategories.FirstOrDefaultAsync(e => e.CategoryId == category.CategoryId && e.BusinessId == category.BusinessId);
                if (categorydb == null)
                    return false;

                categorydb.CategoryName = category.CategoryName;
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CreateMenuCategory(MenuCategoryViewCreation menuCategory)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                MenuCategory newMenuCategory = new MenuCategory()
                {
                    
                    BusinessId = menuCategory.BusinessId,
                    CategoryName = menuCategory.CategoryName,
                };
                db.MenuCategories.Add(newMenuCategory);
                await db.SaveChangesAsync();
                return true;
            }
            

        }
        
        public async Task DeleteMenuCategory(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
               
                var categoria = await db.MenuCategories.FirstOrDefaultAsync(c => c.CategoryId == id);

                if (categoria != null)
                {
                   
                     db.MenuCategories.Remove(categoria);

                    
                    await db.SaveChangesAsync();
                }
            }

        }
    }
}
