using BookStore.Books.Entity;
using BookStore.Books.Entity.CommandEntity;
using BookStore.Books.Entity.QueryEntity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Books.Context
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        public DbSet<BookEntity> BookTable { get; set; }
        

    }
}
