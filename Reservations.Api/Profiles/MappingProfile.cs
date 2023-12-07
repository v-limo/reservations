using AutoMapper;
using Reservations.API.DTO;
using Reservations.API.Model;

namespace Reservations.Api.Profiles;

class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>(); // .ReverseMap();
        CreateMap<BookDto, Book>();
    }
}
