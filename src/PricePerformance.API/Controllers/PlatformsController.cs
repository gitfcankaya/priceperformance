using Microsoft.AspNetCore.Mvc;
using PricePerformance.Core.Entities;
using PricePerformance.Core.Interfaces;

namespace PricePerformance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IRepository<Platform> _repository;

    public PlatformsController(IRepository<Platform> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
    {
        var platforms = await _repository.GetAllAsync();
        return Ok(platforms.Where(p => p.IsActive));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Platform>> GetPlatform(int id)
    {
        var platform = await _repository.GetByIdAsync(id);
        if (platform == null)
            return NotFound();

        return Ok(platform);
    }
}
