using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccess.Interfaces;
using WebApi.Models.Entities;

namespace WebApi.DataAccess.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;

        public MovieRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var query = _context.Movies.Include(p => p.Actors).Include(p => p.Directors).Include(p => p.Genres).AsQueryable().AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<Movie> GetMovie(int movieId)
        {
            return await _context.Movies
                .Include(p => p.Actors)
                .Include(p => p.Directors)
                .Include(p => p.Genres)
                .Where(movie => movie.Id == movieId).FirstOrDefaultAsync();
        }
    }
}
