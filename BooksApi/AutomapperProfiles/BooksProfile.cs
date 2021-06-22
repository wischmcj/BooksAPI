using AutoMapper;
using BooksApi.Data;
using BooksApi.Models.Books;


namespace BooksApi.AutomapperProfiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            // From -> To (src => destination)
            CreateMap<Book, GetBookSummaryItemResponse>();

            CreateMap<Book, GetBookResponse>().ForMember(dest => dest.Author,
                config => config.MapFrom(src => src.Author.ToUpper()));

            CreateMap<PostBookRequest, Book>().ForMember(dest => dest.RemovedFromInventory,
                config => config.MapFrom(_ => false));
        }
    }
}
