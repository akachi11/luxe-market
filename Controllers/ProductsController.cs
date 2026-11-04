using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxeMarket.Data;
using LuxeMarket.Models;

namespace LuxeMarket.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? brandId,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .AsQueryable();

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search));

        query = sortBy switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name" => query.OrderBy(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var totalItems = await query.CountAsync();

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            data = products,
            page,
            pageSize,
            totalItems,
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        product.Name = updatedProduct.Name;
        product.BrandId = updatedProduct.BrandId;
        product.Price = updatedProduct.Price;
        product.CategoryId = updatedProduct.CategoryId;
        product.Description = updatedProduct.Description;
        product.ImageUrl = updatedProduct.ImageUrl;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
