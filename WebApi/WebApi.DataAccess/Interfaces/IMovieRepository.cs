using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Entities;

namespace WebApi.DataAccess.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMovies();
        Task<Movie> GetMovie(int movieId);
    }
}
