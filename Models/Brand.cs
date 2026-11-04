namespace LuxeMarket.Models;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Country { get; set; }
    public List<Product> Products { get; set; } = new();
}