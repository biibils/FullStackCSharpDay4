using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paracommerce.Data;

namespace paracommerce.Controllers;

public class MarketController : Controller
{
    private readonly AppDbContext _context;

    public MarketController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchName)
    {
        var products = await _context
            .Products.Where(s =>
                string.IsNullOrEmpty(searchName) || (s.Name != null && s.Name.Contains(searchName))
            )
            .ToListAsync();

        if (!products.Any())
        {
            ViewBag.Message = "Belum ada produk yang dapat ditampilkan.";
        }
        return View(products);
    }
}
