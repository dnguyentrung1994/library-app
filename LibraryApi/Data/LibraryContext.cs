using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;

namespace LibraryApi.Data
{
  public class LibraryContext: DbContext 
  {
    public LibraryContext (DbContextOptions<LibraryContext> options)
      :base(options)
    {
    }
    public DbSet<User> Users {get; set;} = null!;
    public DbSet<LibraryApi.Data.Book>? Book { get; set; }
  }
}