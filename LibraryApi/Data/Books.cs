using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace LibraryApi.Data
{
  [Table("books")]
  public class Book
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("book_id")]
    public long Id {get; set;}

    [Required]
    [Column("book_title")]
    public string Title {get; set;} = String.Empty;

    [Column("edition")]
    public string Edition {get; set;} = String.Empty;

    [Column("added_at")]
    public DateTime AddedAt {get; set;} =DateTime.Now;

    [Column("disposed_at")]
    public DateTime? DisposedAt {get; set;}

    [Column("borrower_id")]
    public long? UserId {get; set;}

    [Column("borrowed_at")]
    public DateTime? BorrowedAt {get; set;}

    public NpgsqlTsVector SearchVector { get; set; }

    public User? User {get; set;}
  }
}
