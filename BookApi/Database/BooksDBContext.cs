using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Database
{
    public class BooksDBContext : DbContext
    {
        public BooksDBContext(DbContextOptions<BooksDBContext> options)
        : base(options) { }

        public DbSet<Book> Books { get; set; }

    }
}
