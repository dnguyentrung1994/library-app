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
    public DateTime AddedAt {get; set;} =DateTime.Now;

    [Column("disposed_at")]
    public DateTime? DisposedAt {get; set;}

    [Column("borrower_id")]
    public long? UserId {get; set;}

    [Column("borrowed_at")]
    public DateTime? BorrowedAt {get; set;}

    public User? User {get; set;}
  }
}

namespace LibraryApi.DTO
{
  [Serializable]
  public class BriefBookDTO
  {
    public string Title {set; get;} = String.Empty;
    public long? BorrowerId {get; set;}
    public string? BorrowerFirstName {get;set;}
    public string? BorrowerLastName {get;set;}
  }

  [Serializable]
  public class BookDTO
  {
    public string Title {set; get;} = String.Empty;
    public string Edition {get; set;} = String.Empty;
    public DateOnly AddedAt {get; set;}
  }
}