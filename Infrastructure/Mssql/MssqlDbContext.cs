using Infrastructure.Mssql.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mssql;

public class MssqlDbContext : DbContext
{
    public MssqlDbContext(DbContextOptions<MssqlDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<ShopOrder> ShopOrders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<AddOn> AddOns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ ตั้งค่า Composite Primary Key สำหรับ ShopOrder
        modelBuilder.Entity<ShopOrder>()
            .HasKey(so => new { so.OrderId, so.ShopId });

        // ✅ ตั้งค่า Foreign Key สำหรับ ShopOrder
        modelBuilder.Entity<ShopOrder>()
            .HasOne(so => so.Order)
            .WithMany(o => o.ShopOrders)
            .HasForeignKey(so => so.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // ถ้า Order ถูกลบ ให้ลบ ShopOrder ด้วย

        modelBuilder.Entity<ShopOrder>()
            .HasOne(so => so.Shop)
            .WithMany()
            .HasForeignKey(so => so.ShopId)
            .OnDelete(DeleteBehavior.Cascade); // ถ้า Shop ถูกลบ ให้ลบ ShopOrder ด้วย

        // ✅ ตั้งค่า Composite Foreign Key สำหรับ OrderItem -> ShopOrder
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.ShopOrder)
            .WithMany(so => so.OrderItems)
            .HasForeignKey(oi => new { oi.OrderId, oi.ShopId })
            .OnDelete(DeleteBehavior.Cascade); // ถ้า ShopOrder ถูกลบ ให้ลบ OrderItem ด้วย

        // ✅ ตั้งค่า Foreign Key สำหรับ AddOn -> OrderItem
        modelBuilder.Entity<AddOn>()
            .HasOne(a => a.OrderItem)
            .WithMany(oi => oi.AddOns)
            .HasForeignKey(a => a.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade); // ถ้า OrderItem ถูกลบ ให้ลบ AddOn ด้วย

        // ✅ ตั้งค่า Foreign Key สำหรับ Discount -> Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Discount)
            .WithMany()
            .HasForeignKey(o => o.DiscountId)
            .OnDelete(DeleteBehavior.SetNull); // ถ้า Discount ถูกลบ ให้ DiscountId เป็น NULL
    }
}
