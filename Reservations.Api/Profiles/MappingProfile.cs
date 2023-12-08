

namespace Reservations.Api.Profiles;

class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>(); // .ReverseMap();
        CreateMap<BookDto, Book>();
    }
}
