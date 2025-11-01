using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PricePerformance.Core.Entities;
using PricePerformance.Core.Interfaces;
using PricePerformance.Infrastructure.Data;

namespace PricePerformance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IRepository<Category> _repository;
    private readonly ApplicationDbContext _context;

    public CategoriesController(IRepository<Category> repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.ParentCategory)
            .ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpGet("root")]
    public async Task<ActionResult<IEnumerable<Category>>> GetRootCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.SubCategories)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();
        return Ok(categories);
    }
}
