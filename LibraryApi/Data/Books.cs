using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public DateOnly AddedAt {get; set;}

    [Column("disposed_at")]
    public DateOnly? DisposedAt {get; set;}

    [Column("borrower_id")]
    public long? UserId {get; set;}

    public User? User {get; set;}
  }
}