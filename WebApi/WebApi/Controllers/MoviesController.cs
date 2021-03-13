using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Business.Interfaces;
using WebApi.Extensions;
using WebApi.Models.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieAction _movieAction;

        public MoviesController(IMovieAction movieAction)
        {
            _movieAction = movieAction;
        }

        [HttpGet]
        //api/movies
        public async Task<ActionResult<IEnumerable<MovieInfoDto>>> GetMovies([FromQuery] UserParamsDto userParamsDto)
        {
            var movies = await _movieAction.GetMovies(userParamsDto);
            Response.AddPaginationHeader(movies.CurrentPage, movies.PageSize, movies.TotalCount, movies.TotalPages);
            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        public async Task<ActionResult<MovieInfoDto>> GetMovie(int movieId)
        {
            var movie = await _movieAction.GetMovie(movieId);

            if (movie != null)
            {
                return Ok(movie);
            }

            return NotFound();
        }
    }
}
