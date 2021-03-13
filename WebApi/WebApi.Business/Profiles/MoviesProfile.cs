using AutoMapper;
using System.Linq;
using WebApi.Models.Entities;
using WebApi.Models.Models;

namespace WebApi.Business.Profiles
{
    public class MoviesProfile
    {
        public static MapperConfiguration MapAllConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieInfoDto>()
                    .ForMember(dest => dest.ReleasedDate, source => source.MapFrom(src => src.ReleaseDate))
                    .ForMember(dest => dest.Directors, source => source.MapFrom(src => src.Directors.Select(x => x.Name).ToList()))
                    .ForMember(dest => dest.Actors, source => source.MapFrom(src => src.Actors.Select(x => x.Name).ToList()))
                    .ForMember(dest => dest.Genres, source => source.MapFrom(src => src.Genres.Select(x => x.Type).ToList()));
            });

            return configuration;
        }
    }
}
