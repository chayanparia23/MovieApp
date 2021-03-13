using System.Threading.Tasks;
using WebApi.Models.Helpers;
using WebApi.Models.Models;

namespace WebApi.Business.Interfaces
{
    public interface IMovieAction
    {
        Task<PagedList<MovieInfoDto>> GetMovies(UserParamsDto userParamsDto);
        Task<MovieInfoDto> GetMovie(int movieId);
    }
}
