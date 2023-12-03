using BookStore.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.User.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<UserEntity> UserTable { get; set; }

    }
}
