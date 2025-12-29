using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paracommerce.Data;
using paracommerce.Models;

namespace paracommerce.Controllers.Api;

[ApiController]
[ApiVersion("1.0")]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/v1/ProductsApi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // GET: api/v1/ProductsApi/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        return product;
    }

    // PUT: api/v1/ProductsApi/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/v1/ProductsApi/{id}
    [HttpPost("{id}")]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // DELETE: api/v1/ProductsApi/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
