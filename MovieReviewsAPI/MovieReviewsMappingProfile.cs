using AutoMapper;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.Models;

namespace MovieReviewsAPI
{
    public class MovieReviewsMappingProfile : Profile
    {
        public MovieReviewsMappingProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Review, ReviewDto>()
                .ForMember(r => r.UserName, a => a.MapFrom(x => x.Author.UserName));
            CreateMap<Movie, MovieDto>()
                .ForMember(m => m.Director, x => x.MapFrom(d => d.Director.FirstName + " " + d.Director.LastName));
        }
    }
}
