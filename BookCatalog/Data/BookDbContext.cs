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

            // Връзка между Book и Author (1 към Много)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // Ако авторът бъде изтрит, книгите също ще бъдат премахнати

            // Връзка между BookTag и Book/Tag (Много към Много)
            modelBuilder.Entity<BookTag>()
                .HasKey(bt => new { bt.BookId, bt.TagId });

            modelBuilder.Entity<BookTag>()
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BookTags)
                .HasForeignKey(bt => bt.BookId);

            modelBuilder.Entity<BookTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BookTags)
                .HasForeignKey(bt => bt.TagId);

            // Seed данни (по желание)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, Name = "Фантастика" },
                new Genre { GenreId = 2, Name = "Трилър" },
                new Genre { GenreId = 3, Name = "Роман" },
                new Genre { GenreId = 4, Name = "Повест" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "Дж. Р. Р. Толкин" },
                new Author { AuthorId = 2, Name = "Айзък Азимов" },
                new Author { AuthorId = 3, Name = "Стивън Кинг" }
            );
        }
    }
}
