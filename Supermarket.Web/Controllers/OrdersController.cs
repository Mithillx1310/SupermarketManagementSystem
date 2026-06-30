using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders.ToListAsync();

            ViewBag.TotalOrders = orders.Count;
            ViewBag.TotalSales = orders.Sum(o => o.TotalPrice);
            ViewBag.TotalItemsSold = orders.Sum(o => o.Quantity);
            ViewBag.AverageOrder = orders.Count > 0 ? orders.Average(o => o.TotalPrice) : 0;

            return View(orders);
        }

        public async Task<IActionResult> Report()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders.ToListAsync();

            ViewBag.TotalOrders = orders.Count;
            ViewBag.TotalSales = orders.Sum(o => o.TotalPrice);
            ViewBag.TotalItemsSold = orders.Sum(o => o.Quantity);
            ViewBag.AverageOrderValue = orders.Count > 0 ? orders.Average(o => o.TotalPrice) : 0;

            ViewBag.BestSellingProduct = orders
                .GroupBy(o => o.ProductName)
                .OrderByDescending(g => g.Sum(x => x.Quantity))
                .Select(g => g.Key)
                .FirstOrDefault() ?? "No orders yet";

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Title.ToLower().Trim() == order.ProductName.ToLower().Trim());

            if (product == null)
            {
                TempData["Error"] = "Product not found. Please enter the exact product name.";
                return RedirectToAction("Create");
            }

            if (order.Quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than 0.";
                return RedirectToAction("Create");
            }

            if (product.QuantityInStock < order.Quantity)
            {
                TempData["Error"] = "Not enough stock. Available stock: " + product.QuantityInStock;
                return RedirectToAction("Create");
            }

            product.QuantityInStock -= order.Quantity;
            product.StockAvailabilityStatus = GetStockStatus(product.QuantityInStock);

            _context.Products.Update(product);
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Order created successfully. Product stock updated.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var oldOrder = await _context.Orders.AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (oldOrder == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index");
            }

            var oldProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Title.ToLower().Trim() == oldOrder.ProductName.ToLower().Trim());

            if (oldProduct != null)
            {
                oldProduct.QuantityInStock += oldOrder.Quantity;
                oldProduct.StockAvailabilityStatus = GetStockStatus(oldProduct.QuantityInStock);
                _context.Products.Update(oldProduct);
                await _context.SaveChangesAsync();
            }

            var newProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Title.ToLower().Trim() == order.ProductName.ToLower().Trim());

            if (newProduct == null)
            {
                TempData["Error"] = "Product not found. Please enter the exact product name.";
                return RedirectToAction("Edit", new { id = order.OrderId });
            }

            if (newProduct.QuantityInStock < order.Quantity)
            {
                TempData["Error"] = "Not enough stock. Available stock: " + newProduct.QuantityInStock;
                return RedirectToAction("Edit", new { id = order.OrderId });
            }

            newProduct.QuantityInStock -= order.Quantity;
            newProduct.StockAvailabilityStatus = GetStockStatus(newProduct.QuantityInStock);

            _context.Products.Update(newProduct);
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Order updated successfully. Product stock updated.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Order order)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var existingOrder = await _context.Orders.FindAsync(order.OrderId);

            if (existingOrder != null)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Title.ToLower().Trim() == existingOrder.ProductName.ToLower().Trim());

                if (product != null)
                {
                    product.QuantityInStock += existingOrder.Quantity;
                    product.StockAvailabilityStatus = GetStockStatus(product.QuantityInStock);
                    _context.Products.Update(product);
                }

                _context.Orders.Remove(existingOrder);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Order deleted successfully. Product stock restored.";
            }

            return RedirectToAction("Index");
        }

        private string GetStockStatus(int quantity)
        {
            if (quantity == 0)
                return "Out of Stock";
            else if (quantity <= 10)
                return "Low Stock";
            else
                return "In Stock";
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }
    }
}