using System.ComponentModel.DataAnnotations;
namespace LibraryApi.DTO
{
  public class UserWithBookDTO
  {
    public long Id {get; set;}

    [Required]
    public string FirstName {get; set;} = String.Empty;

    public string LastName {get; set;} = String.Empty;

    public string Address {get; set;} = String.Empty;

    public string PhoneNumber {get; set;} = String.Empty;

    public bool IsStaff {get; set;} = false;

    public bool IsActive {get; set;} = true;

    public List<InnerBookDTO> Books {get; set;} = new List<InnerBookDTO>();
  }

  public class UserDTO
  {
    public long Id {get; set;}

    [Required]
    public string FirstName {get; set;} = String.Empty;

    public string LastName {get; set;} = String.Empty;

    public string Address {get; set;} = String.Empty;

    public string PhoneNumber {get; set;} = String.Empty;

    public bool IsStaff {get; set;} = false;

    public bool IsActive {get; set;} = true;
  }
}