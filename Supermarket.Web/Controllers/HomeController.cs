using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Username") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var products = await _context.Products.ToListAsync();
        var suppliers = await _context.Suppliers.ToListAsync();
        var orders = await _context.Orders.ToListAsync();

        DateTime today = DateTime.Today;
        DateTime next30Days = today.AddDays(30);

        ViewBag.TotalProducts = products.Count;
        ViewBag.TotalSuppliers = suppliers.Count;
        ViewBag.TotalOrders = orders.Count;
        ViewBag.TotalSales = orders.Sum(o => o.TotalPrice);
        ViewBag.LowStockProducts = products.Count(p => p.QuantityInStock <= 10);
        ViewBag.ExpiringSoonProducts = products.Count(p =>
            p.ExpiryDate >= today && p.ExpiryDate <= next30Days);

        return View(orders.OrderByDescending(o => o.OrderDate).Take(5).ToList());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
