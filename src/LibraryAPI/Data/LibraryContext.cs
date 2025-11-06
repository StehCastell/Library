using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionBook> CollectionBooks { get; set; }
        public DbSet<CollectionAuthor> CollectionAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(e => e.Books)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(e => e.Books)
                      .HasForeignKey(e => e.UserId);
            });

            // Configure BookAuthor entity (Many-to-Many relationship)
            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasIndex(e => new { e.BookId, e.AuthorId }).IsUnique();

                entity.HasOne(e => e.Book)
                      .WithMany(e => e.BookAuthors)
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Author)
                      .WithMany(e => e.BookAuthors)
                      .HasForeignKey(e => e.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Collection entity
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CollectionBook entity (Many-to-Many relationship)
            modelBuilder.Entity<CollectionBook>(entity =>
            {
                entity.HasIndex(e => new { e.CollectionId, e.BookId }).IsUnique();

                entity.HasOne(e => e.Collection)
                      .WithMany(e => e.CollectionBooks)
                      .HasForeignKey(e => e.CollectionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Book)
                      .WithMany(e => e.CollectionBooks)
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CollectionAuthor entity (Many-to-Many relationship)
            modelBuilder.Entity<CollectionAuthor>(entity =>
            {
                entity.HasIndex(e => new { e.CollectionId, e.AuthorId }).IsUnique();

                entity.HasOne(e => e.Collection)
                      .WithMany(e => e.CollectionAuthors)
                      .HasForeignKey(e => e.CollectionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Author)
                      .WithMany(e => e.CollectionAuthors)
                      .HasForeignKey(e => e.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}