using Microsoft.AspNetCore.Mvc;
using PricePerformance.Core.Entities;
using PricePerformance.Core.Interfaces;

namespace PricePerformance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly IRepository<Country> _repository;

    public CountriesController(IRepository<Country> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        var countries = await _repository.GetAllAsync();
        return Ok(countries.Where(c => c.IsActive));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        var country = await _repository.GetByIdAsync(id);
        if (country == null)
            return NotFound();

        return Ok(country);
    }
}
