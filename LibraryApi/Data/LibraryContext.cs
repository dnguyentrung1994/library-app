using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
  public class LibraryContext: DbContext 
  {
    public LibraryContext (DbContextOptions<LibraryContext> options)
      :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Book>()
        .HasGeneratedTsVectorColumn(
          b => b.SearchVector,
          "english",
          b => new {b.Title})
        .HasIndex(b=>b.SearchVector)
        .HasMethod("GIN");

      modelBuilder.Entity<Book>()
        .HasOne(b=>b.User)
        .WithMany(u=>u.Books);
    }
    public DbSet<User> Users {get; set;} = null!;
    public DbSet<LibraryApi.Data.Book>? Book { get; set; }
  }
}
