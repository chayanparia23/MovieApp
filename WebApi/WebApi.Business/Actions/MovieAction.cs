using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Business.Interfaces;
using WebApi.Business.Profiles;
using WebApi.DataAccess.Interfaces;
using WebApi.Models.Helpers;
using WebApi.Models.Models;

namespace WebApi.Business.Actions
{
    public class MovieAction : IMovieAction
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration configuration;

        public MovieAction(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
            configuration = MoviesProfile.MapAllConfiguration();
            _mapper = configuration.CreateMapper();
        }

        public async Task<PagedList<MovieInfoDto>> GetMovies(UserParamsDto userParamsDto)
        {
            var dbMovies = await _movieRepository.GetMovies();

            if (!string.IsNullOrWhiteSpace(userParamsDto.SearchText) && !string.IsNullOrWhiteSpace(userParamsDto.SearchTextType))
            {
                switch (userParamsDto.SearchTextType)
                {
                    case "Year":
                        dbMovies = dbMovies.Where(x => x.Year.ToString().Contains(userParamsDto.SearchText));
                        break;
                    case "Plot":
                        dbMovies = dbMovies.Where(x => !string.IsNullOrWhiteSpace(x.Plot) && x.Plot.Contains(userParamsDto.SearchText));
                        break;
                    case "Title":
                        dbMovies = dbMovies.Where(x => !string.IsNullOrWhiteSpace(x.Title) && x.Title.Contains(userParamsDto.SearchText));
                        break;
                    case "Genre":
                        dbMovies = dbMovies.Where(x => x.Genres.Select(x => x.Type).ToList().Contains(userParamsDto.SearchText));
                        break;
                    case "Actor":
                        dbMovies = dbMovies.Where(x => x.Actors.Select(x => x.Name).ToList().Contains(userParamsDto.SearchText));
                        break;
                    case "Director":
                        dbMovies = dbMovies.Where(x => x.Directors.Select(x => x.Name).ToList().Contains(userParamsDto.SearchText));
                        break;
                } 
            }

            dbMovies = dbMovies.OrderByDescending(x => x.Year).ToList();

            var totalCount = dbMovies.Count();
            dbMovies = dbMovies.Skip((userParamsDto.PageNumber - 1) * userParamsDto.PageSize).Take(userParamsDto.PageSize).ToList();

            var listOfMoviesInfo = _mapper.Map<IEnumerable<MovieInfoDto>>(dbMovies);

            return new PagedList<MovieInfoDto>(listOfMoviesInfo, totalCount, userParamsDto.PageNumber, userParamsDto.PageSize);
        }

        public async Task<MovieInfoDto> GetMovie(int movieId)
        {
            var dbMovie = await _movieRepository.GetMovie(movieId);

            var movie = _mapper.Map<MovieInfoDto>(dbMovie);

            return movie;
        }
    }
}
