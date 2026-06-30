using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchText, string category)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var allProducts = await _context.Products.ToListAsync();

            ViewBag.TotalProducts = allProducts.Count;
            ViewBag.InStockProducts = allProducts.Count(p => p.StockAvailabilityStatus == "In Stock");
            ViewBag.LowStockProducts = allProducts.Count(p => p.StockAvailabilityStatus == "Low Stock");
            ViewBag.OutOfStockProducts = allProducts.Count(p => p.StockAvailabilityStatus == "Out of Stock");

            DateTime today = DateTime.Today;
            DateTime next30Days = today.AddDays(30);

            ViewBag.ExpiringSoonProducts = allProducts.Count(p =>
                p.ExpiryDate >= today && p.ExpiryDate <= next30Days);

            ViewBag.Categories = allProducts
                .Select(p => p.ProductCategory)
                .Distinct()
                .ToList();

            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p =>
                    p.Title.Contains(searchText) ||
                    p.Barcode.Contains(searchText) ||
                    p.BrandSupplier.Contains(searchText) ||
                    p.ProductCategory.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.ProductCategory == category);
            }

            ViewBag.SearchText = searchText;
            ViewBag.SelectedCategory = category;

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> LowStock()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var lowStockProducts = await _context.Products
                .Where(p => p.QuantityInStock < 10)
                .OrderBy(p => p.QuantityInStock)
                .ToListAsync();

            return View(lowStockProducts);
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
        public async Task<IActionResult> Create(Product product)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            product.StockAvailabilityStatus = GetStockStatus(product.QuantityInStock);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product added successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            product.StockAvailabilityStatus = GetStockStatus(product.QuantityInStock);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product updated successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var existingProduct = await _context.Products.FindAsync(product.ProductId);

            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Product could not be found.";
            }

            return RedirectToAction("Index");
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        private string GetStockStatus(int quantity)
        {
            if (quantity == 0)
            {
                return "Out of Stock";
            }
            else if (quantity <= 10)
            {
                return "Low Stock";
            }
            else
            {
                return "In Stock";
            }
        }
    }
}