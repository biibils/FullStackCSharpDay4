using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paracommerce.Data;
using paracommerce.Models;

namespace paracommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchName)
        {
            var products = await _context
                .Products.Where(s =>
                    string.IsNullOrEmpty(searchName)
                    || (s.Name != null && s.Name.Contains(searchName))
                )
                .ToListAsync();

            if (!products.Any())
            {
                ViewBag.Message = "Belum ada produk yang ditambahkan.";
            }
            return View(products);
        }

        // GET: Product/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Product/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Produk berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }
            return View(product);
        }

        // POST: Product/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product input)
        {
            if (id != input.Id)
            {
                TempData["ErrorMessage"] = "ID Produk tidak valid";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(input);

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produk tidak ditemukan";
                return RedirectToAction(nameof(Index));
            }

            // Update field secara eksplisit
            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.Stock = input.Stock;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data produk berhasil diperbarui";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Data telah diubah oleh proses lain";
                return RedirectToAction(nameof(Edit), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }
            return View(product);
        }

        // POST: Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data Produk berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produk tidak ditemukan";
                return RedirectToAction("Index", "Product");
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
