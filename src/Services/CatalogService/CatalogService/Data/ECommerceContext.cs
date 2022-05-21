using CatalogService.Data.Config;
using CatalogService.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogService.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasData(
                    new Product { Name = "Apple Phone", Price = 12000, Id = 1 },
                    new Product { Name = "Samsung Phone", Price = 5000, Id = 2 },
                    new Product { Name = "LG Phone", Price = 7000, Id = 3 },
                    new Product { Name = "Xaomi Phone", Price = 10000, Id = 4 },
                    new Product { Name = "Nokia Phone", Price = 2000, Id = 5 });
        }

        public class DBContextFactory : IDesignTimeDbContextFactory<ECommerceContext>
        {
            ECommerceContext IDesignTimeDbContextFactory<ECommerceContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ECommerce;User Id=sa;Password=sql;");

                return new ECommerceContext(optionsBuilder.Options);
            }
        }
    }
}