using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxeMarket.Data;
using LuxeMarket.Models;

namespace LuxeMarket.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BrandsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var brands = await _context.Brands.ToListAsync();
        return Ok(brands);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _context.Brands
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
            return NotFound();

        return Ok(brand);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Brand brand)
    {
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
    }
}