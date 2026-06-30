using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View(await _context.Suppliers.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Supplier added successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Supplier updated successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Supplier supplier)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var existingSupplier = await _context.Suppliers.FindAsync(supplier.SupplierId);

            if (existingSupplier != null)
            {
                _context.Suppliers.Remove(existingSupplier);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Supplier deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Supplier could not be found.";
            }

            return RedirectToAction("Index");
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }
    }
}