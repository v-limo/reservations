
namespace Reservations.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>(); // .ReverseMap();
        CreateMap<BookDto, Book>();

        CreateMap<CreateBookDto, Book>().ReverseMap();
        CreateMap<UpdateBookDto, Book>().ReverseMap();

        CreateMap<ReservationHistoryDto, ReservationHistory>().ReverseMap();
    }
}
