using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PricePerformance.Core.Entities;
using PricePerformance.Core.Interfaces;
using PricePerformance.Infrastructure.Data;

namespace PricePerformance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<Product> _repository;
    private readonly ApplicationDbContext _context;

    public ProductsController(IRepository<Product> repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int? categoryId)
    {
        if (categoryId.HasValue)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductPrices)
                    .ThenInclude(pp => pp.Platform)
                .Include(p => p.ProductPrices)
                    .ThenInclude(pp => pp.Country)
                .Where(p => p.CategoryId == categoryId.Value && p.IsActive)
                .ToListAsync();
            return Ok(products);
        }

        var allProducts = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Platform)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Country)
            .Where(p => p.IsActive)
            .ToListAsync();
        return Ok(allProducts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Platform)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Country)
            .Include(p => p.Reviews)
            .Include(p => p.Specifications)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Search query cannot be empty");

        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && 
                   (p.Name.Contains(query) || 
                    p.Brand != null && p.Brand.Contains(query) ||
                    p.Model != null && p.Model.Contains(query)))
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var createdProduct = await _repository.AddAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        await _repository.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
