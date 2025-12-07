using greengrocer.Models;
using Microsoft.EntityFrameworkCore;

namespace greengrocer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");       // tablo adı
                entity.HasKey(o => o.OrderId); // PK
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");  
                entity.HasKey(oi => oi.Id);   

                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(oi => oi.OrderId);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");
            });
        }

    }
}
