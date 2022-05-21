using LoginService.Data.Config;
using LoginService.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LoginService.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasData(
                    new User { UserName = "ugur", Password = "1234", Id = 1, Email = "ugurmutlucan15@hotmail.com" },
                    new User { UserName = "ugur1", Password = "1234", Id = 2, Email = "ugurmutlucan15@hotmail.com" },
                    new User { UserName = "ugur2", Password = "1234", Id = 3, Email = "ugurmutlucan15@hotmail.com" });
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