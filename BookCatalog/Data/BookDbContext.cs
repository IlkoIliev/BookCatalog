using BookCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookTag> BookTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed данни (по желание)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, Name = "Фантастика" },
                new Genre { GenreId = 2, Name = "Трилър" },
                new Genre { GenreId = 3, Name = "Роман" },
                new Genre { GenreId = 3, Name = "Повест" }
            );
        }

    }
}
