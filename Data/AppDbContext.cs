using Microsoft.EntityFrameworkCore;
using LuxeMarket.Models;

namespace LuxeMarket.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}