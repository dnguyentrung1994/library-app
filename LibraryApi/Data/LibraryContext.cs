using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
  public class LibraryContext: DbContext 
  {
    public LibraryContext (DbContextOptions<LibraryContext> options)
      :base(options)
    {
    }
    public DbSet<User> Users {get; set;} = null!;
  }
}