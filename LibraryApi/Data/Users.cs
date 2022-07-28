using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Data{
  [Table("users")]
  public class User 
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public long Id {get; set;}
    [Required]
    [Column("first_name")]
    public string FirstName {get; set;} = String.Empty;

    [Column("last_name")]
    public string? LastName {get; set;} 

    [Column("address")]
    public string Address {get; set;} = String.Empty;

    [Column("phone_number")]
    public string PhoneNumber {get; set;} = String.Empty;

    [Column("is_staff")]
    public bool IsStaff {get; set;} = false;

    [Column("is_active")]
    public bool IsActive {get; set;} = true;

    public List<Book> Books {get; set;} = new List<Book>();
  }
}
