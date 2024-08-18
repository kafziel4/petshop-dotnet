using Microsoft.EntityFrameworkCore;
using PetShop.Entities;

namespace PetShop.DbContexts;

public class PetShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public PetShopDbContext(DbContextOptions<PetShopDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .OwnsMany(p => p.Variants, v =>
            {
                v.WithOwner().HasForeignKey("ProductId");
                v.Property<int>("Id");
                v.HasKey("Id");
            });
    }
}
