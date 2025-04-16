using Kaboom.Models.Admin;
using Kaboom.Models.AuthUserModel;
using Kaboom.Models.Features;
using Kaboom.Models.Order;
using Kaboom.Models.product;
using Kaboom.Models.StockModel;
using Kaboom.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Kaboom.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Users.Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<AuthUser> AuthUser { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<PreferenceToggle> PreferenceToggle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users.Users>()
            .HasOne(u => u.Admin)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.AdminId)
            .OnDelete(DeleteBehavior.Cascade);
            // Define relationships
            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Orders)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<AuthUser>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.AuthUser)
                .HasForeignKey(rt => rt.AuthUserId)
                .OnDelete(DeleteBehavior.Cascade);
            //This configuration specifies that each `Stock` is associated with one `Product`
            //, and the foreign key in `Stocks` is `ProductId`. If a `Product` is deleted
            //, all associated `Stocks` will also be deleted (`DeleteBehavior.Cascade`).
            modelBuilder.Entity<Stocks>()
            .HasOne(s => s.Products)
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
