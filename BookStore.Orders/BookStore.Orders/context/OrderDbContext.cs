using BookStore.Orders.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Orders.context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
        public DbSet<OrderEntity> OrderTable { get; set; }

    }
}
