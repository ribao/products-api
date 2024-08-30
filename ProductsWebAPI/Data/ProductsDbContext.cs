using Microsoft.EntityFrameworkCore;
using ProductsWebAPI.Model;

namespace ProductsWebAPI.Data;

// Data/ProductsDbContext.cs

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}