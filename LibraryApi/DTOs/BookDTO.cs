using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTO
{
  [Serializable]
  public class BookWithUserDTO
  {
    public long Id {set; get;}

    public string Title {set; get;} = String.Empty;

    public string Edition {get; set;} = String.Empty;

    public DateTime AddedAt {get; set;} =DateTime.Now;

    public DateTime? DisposedAt {get; set;}

    public DateTime? BorrowedAt {get; set;}

    public UserDTO? User {set; get;}
  }

  [Serializable]
  public class BookDTO
  {
    public long Id {set; get;}

    public string Title {set; get;} = String.Empty;

    public string Edition {get; set;} = String.Empty;

    public DateTime AddedAt {get; set;} =DateTime.Now;

    public DateTime? DisposedAt {get; set;}

    public DateTime? BorrowedAt {get; set;}
  }

  [Serializable]
  public class InnerBookDTO
  {
    public long Id {set; get;}
    public string Title {set; get;} = String.Empty;
    public string Edition {get; set;} = String.Empty;
    public DateTime BorrowedAt {get; set;}
  }

  public class CreateBookDTO 
  {
    [Required]
    public string Title {set; get;} = String.Empty;
    public string Edition {get; set;} = String.Empty;
  }

  public class ModifyBookDTO
  {
    public string? Title {get; set;}

    public string? Edition {get; set;}
  }
}