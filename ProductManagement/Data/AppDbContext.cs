using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using System.Reflection.Emit;

namespace ProductManagement.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Products = Set<ProductModel>();
        }

        public DbSet<ProductModel> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .Property(e => e.FirstName)
                .HasMaxLength(255);
            modelBuilder.Entity<UserModel>()
            .Property(e => e.LastName)
            .HasMaxLength(255);
        }

    }
}
