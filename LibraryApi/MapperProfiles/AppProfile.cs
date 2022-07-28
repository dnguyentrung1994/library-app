using AutoMapper;
using LibraryApi.Data;
using LibraryApi.DTO;

namespace LibraryApi.Profiles
{
  public class AppProfile: Profile
    {
      public AppProfile()
      {
        CreateMap<Book, BookWithUserDTO>();
        CreateMap<BookWithUserDTO, Book>();
        CreateMap<Book, InnerBookDTO>();
        CreateMap<Book, BookDTO>();
        CreateMap<BookDTO, Book>();
        CreateMap<User, UserWithBookDTO>();
        CreateMap<UserWithBookDTO, User>();
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
      }
    }
}