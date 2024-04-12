using PruebaTecnicaABSolutions.Models;
using Microsoft.EntityFrameworkCore;


namespace PruebaTecnicaABSolutions.Services
{
    public interface IMenuItemsService
    {
        Task CreateMenuItem(MenuItemViewCreation item);
        Task<bool> DeleteMenuItemById(int id);
        Task<bool> DeleteMenuItemByidandBussines(int id, int bussinesid);
        Task<IEnumerable<MenuItemView>> FindAllMenuItems();
        Task<IEnumerable<MenuItemView>> FindAllMenuItemsByBusiness(int idBussiness);
        Task<MenuItemView?> FindOneItemByid(int id);
        Task<MenuItemView?> FindOneItemByidandBussines(int id, int idBussiness);
     
        Task<IEnumerable<MenuCategoryViewList?>> MenuCategoryViewList(int idBussiness);
        Task<bool> UpdateMenuItem(MenuItemViewUpdate item);
    }
    public class MenuItemsService : IMenuItemsService
    {
        public async Task<IEnumerable<MenuItemView>> FindAllMenuItems()
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return  await  db.MenuItems
                       .Join(db.MenuCategories,
                              menuItem => menuItem.CategoryId,
                              category => category.CategoryId,
                              (menuItem, category) => new { MenuItem = menuItem, Category = category })
                        .Join(db.Businesses,
                              joined => joined.MenuItem.BusinessId,
                              business => business.BusinessId,
                              (joined, business) => new MenuItemView
                              {
                                  ItemId = joined.MenuItem.ItemId,
                                  ItemName = joined.MenuItem.ItemName,
                                  Description = joined.MenuItem.Description,
                                  Price = joined.MenuItem.Price,
                                  BusinessId = joined.MenuItem.BusinessId,
                                  AddedDate = joined.MenuItem.AddedDate,
                                  CategoryName = joined.Category.CategoryName,
                                  BusinessName = business.BusinessName 
                              })
                        .ToListAsync();

            }
        }

        public async Task<IEnumerable<MenuItemView>> FindAllMenuItemsByBusiness(int idBussiness)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                return await db.MenuItems
                        .Join(db.MenuCategories,
                              menuItem => menuItem.CategoryId,
                              category => category.CategoryId,
                              (menuItem, category) => new MenuItemView
                              {
                                  ItemId = menuItem.ItemId,
                                  ItemName = menuItem.ItemName,
                                  Description = menuItem.Description,
                                  Price = menuItem.Price,
                                  BusinessId = menuItem.BusinessId,
                                  AddedDate = menuItem.AddedDate,
                                  CategoryName = category.CategoryName
                              })
                         .Where(e => e.BusinessId == idBussiness)
                         .ToListAsync();
            }
        }

        public async Task<MenuItemView?> FindOneItemByid(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                var menuItem = await db.MenuItems.FirstOrDefaultAsync(e => e.ItemId == id);

                if (menuItem != null)
                {
                    return await db.MenuItems
                        .Where(item => item.ItemId == id)
                        .Join(db.MenuCategories,
                              item => item.CategoryId,
                              category => category.CategoryId,
                              (item, category) => new { MenuItem = item, Category = category })
                        .Join(db.Businesses,
                              joined => joined.MenuItem.BusinessId,
                              business => business.BusinessId,
                              (joined, business) => new MenuItemView
                              {
                                  CategoryId = joined.MenuItem.CategoryId,
                                  ItemId = joined.MenuItem.ItemId,
                                  ItemName = joined.MenuItem.ItemName,
                                  Description = joined.MenuItem.Description,
                                  Price = joined.MenuItem.Price,
                                  BusinessId = joined.MenuItem.BusinessId,
                                  AddedDate = joined.MenuItem.AddedDate,
                                  CategoryName = joined.Category.CategoryName,
                                  BusinessName = business.BusinessName
                              })
                        .FirstOrDefaultAsync(); 

                }
                else
                {
                    
                    return null; 
                }
            }
        }

        public async Task<MenuItemView?> FindOneItemByidandBussines(int id, int idBussiness)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                var menuItem = await db.MenuItems.FirstOrDefaultAsync(e => e.ItemId == id);

                if (menuItem != null)
                {
                    return await db.MenuItems
                        .Where(item => item.ItemId == id && item.BusinessId == idBussiness)
                        .Join(db.MenuCategories,
                              item => item.CategoryId,
                              category => category.CategoryId,
                              (item, category) => new { MenuItem = item, Category = category })
                        .Join(db.Businesses,
                              joined => joined.MenuItem.BusinessId,
                              business => business.BusinessId,
                              (joined, business) => new MenuItemView
                              {
                                  CategoryId = joined.MenuItem.CategoryId,
                                  ItemId = joined.MenuItem.ItemId,
                                  ItemName = joined.MenuItem.ItemName,
                                  Description = joined.MenuItem.Description,
                                  Price = joined.MenuItem.Price,
                                  BusinessId = joined.MenuItem.BusinessId,
                                  AddedDate = joined.MenuItem.AddedDate,
                                  CategoryName = joined.Category.CategoryName,
                                  BusinessName = business.BusinessName
                              })
                        .FirstOrDefaultAsync();

                }
                else
                {

                    return null;
                }
            }
        }

        public async Task<IEnumerable<MenuCategoryViewList?>> MenuCategoryViewList(int idBussiness)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                if (idBussiness == null) return null;
                return await db.MenuCategories
                    .Where(e => e.BusinessId == idBussiness)
                    .Select(
                    e => new MenuCategoryViewList
                    {
                        CategoryId = e.CategoryId,
                        CategoryName = e.CategoryName
                    })
                    .ToListAsync();
            }
        }


        public async Task<bool> UpdateMenuItem(MenuItemViewUpdate item)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                if (item.ItemId == null)
                    return false;
                
                var menuItem = await db.MenuItems.FirstOrDefaultAsync(e => e.ItemId == item.ItemId);

                if (menuItem != null)
                {
                    menuItem.ItemName = item.ItemName;
                    menuItem.Description = item.Description;
                    menuItem.Price = item.Price;
                    menuItem.CategoryId = item.CategoryId;
                

                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<bool> DeleteMenuItemById(int id)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                var menuItem = await db.MenuItems.FirstOrDefaultAsync(e => e.ItemId == id );

                if (menuItem != null)
                {
                    // Eliminar el objeto
                    db.MenuItems.Remove(menuItem);

                    // Guardar los cambios
                    await db.SaveChangesAsync();
                    return true;
                }

                return false;


            }
        }

        public async Task<bool> DeleteMenuItemByidandBussines(int id, int bussinesid)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {

                var menuItem = await db.MenuItems.FirstOrDefaultAsync(e => e.ItemId == id && e.BusinessId == bussinesid);

                if (menuItem != null)
                {
                    // Eliminar el objeto
                    db.MenuItems.Remove(menuItem);

                    // Guardar los cambios
                    await db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        public async Task CreateMenuItem(MenuItemViewCreation item)
        {
            using (ABPruebaTecnicaContext db = new ABPruebaTecnicaContext())
            {
                MenuItem menuItem = new MenuItem()
                {
                    ItemName = item.ItemName,
                    Price = item.Price,
                    Description = item.Description,
                    BusinessId = item.BusinessId,
                    CategoryId = item.CategoryId,
                };

                db.MenuItems.Add(menuItem);
                await db.SaveChangesAsync();
            }
        }

    }
}
