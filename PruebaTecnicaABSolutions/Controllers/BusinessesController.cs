using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaABSolutions.Models;
using PruebaTecnicaABSolutions.Services;

namespace PruebaTecnicaABSolutions.Controllers
{
    [Authorize]
    public class BusinessesController : Controller
    {
        private readonly IBusinessService businessService;

        public BusinessesController(IBusinessService businessService)
        {
          
            this.businessService = businessService;
        }

        [Authorize(Roles ="1,2")]
        // GET: Businesses
        public async Task<IActionResult> Index()
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            

            if (role == "1")
            {
                var business = await businessService.GetBusinesses();
                return View(business);
            }

            return RedirectToAction("Details");
            
            
        }
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Details(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                var bussinessToShow = await businessService.GetOneBusinesses(id);

                return View(bussinessToShow);

            }
            if (role == "2")
            {
                var currentBussinessToShow = await businessService.GetOneBusinesses(id_B);
                return View(currentBussinessToShow);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(int id)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            

            if (role == "1")
            {
                var bussinessToShow = await businessService.GetOneBusinesses(id);

                return View(bussinessToShow);

            }
           

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(Business business)
        {
            var data = HttpContext.User.Claims.ToList();
            var role = data[2].Value;
            var businees = data[3].Value;
            if (!int.TryParse(businees, out int id_B)) { }

            if (role == "1")
            {
                await businessService.UpdateBusinesses(business);

            }
            if (role == "2")
            {
                if (id_B == business.BusinessId)
                    await businessService.UpdateBusinesses(business);


            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public async Task<IActionResult> Create(Business business)
        {
            await businessService.CreateBusinesses(business);
            return RedirectToAction("Index");
        }
        

        
        [Authorize(Roles = "1")]
        public IActionResult Create()
        {
            Business business = new Business();
            return View(business);
        }
        
        [Authorize(Roles = "1")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await businessService.DeleteBusinesses(id);
            return RedirectToAction("Index");
        }
        //// GET: Businesses/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Businesses/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BusinessId,BusinessName,Description,CreationDate,Address,Email,Phone")] Business business)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(business);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(business);
        //}

        //// GET: Businesses/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Businesses == null)
        //    {
        //        return NotFound();
        //    }

        //    var business = await _context.Businesses.FindAsync(id);
        //    if (business == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(business);
        //}

        //// POST: Businesses/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BusinessId,BusinessName,Description,CreationDate,Address,Email,Phone")] Business business)
        //{
        //    if (id != business.BusinessId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(business);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BusinessExists(business.BusinessId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(business);
        //}

        //// GET: Businesses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Businesses == null)
        //    {
        //        return NotFound();
        //    }

        //    var business = await _context.Businesses
        //        .FirstOrDefaultAsync(m => m.BusinessId == id);
        //    if (business == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(business);
        //}

        //// POST: Businesses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Businesses == null)
        //    {
        //        return Problem("Entity set 'ABPruebaTecnicaContext.Businesses'  is null.");
        //    }
        //    var business = await _context.Businesses.FindAsync(id);
        //    if (business != null)
        //    {
        //        _context.Businesses.Remove(business);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BusinessExists(int id)
        //{
        //  return (_context.Businesses?.Any(e => e.BusinessId == id)).GetValueOrDefault();
        //}
    }
}
