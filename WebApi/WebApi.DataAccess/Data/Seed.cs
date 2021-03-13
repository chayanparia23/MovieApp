using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Models.Entities;
using FileEntities = WebApi.Models.DeserializeEntities;

namespace WebApi.DataAccess.Data
{
    public class Seed
    {
        public static async Task SeedMovies(DataContext context)
        {
            var movieData = await System.IO.File.ReadAllTextAsync("../WebAPI.DataAccess/Data/moviedata.json");
            var movies = JsonSerializer.Deserialize<List<FileEntities.Movie>>(movieData);

            foreach (var movie in movies)
            {
                Movie dbMovie = new Movie()
                {
                    Year = movie.year,
                    Title = movie.title,
                    ReleaseDate = movie.info.release_date,
                    Rating = movie.info.rating,
                    ImageUrl = movie.info.image_url,
                    Plot = movie.info.plot,
                    Rank = movie.info.rank,
                    RunningTimeSecs = movie.info.running_time_secs
                };

                List<Actor> actors = new List<Actor>();
                if(movie.info.actors != null)
                {
                    foreach (var actorEl in movie.info.actors)
                    {
                        actors.Add(new Actor() { Name = actorEl });
                    }
                }
                dbMovie.Actors = actors;

                List<Director> directors = new List<Director>();
                if(movie.info.directors != null)
                {
                    foreach (var directorEl in movie.info.directors)
                    {
                        directors.Add(new Director() { Name = directorEl });
                    }
                }
                dbMovie.Directors = directors;

                List<Genre> genres = new List<Genre>();
                if (movie.info.genres != null)
                {
                    foreach (var genreEl in movie.info.genres)
                    {
                        genres.Add(new Genre() { Type = genreEl });
                    }
                }
                dbMovie.Genres = genres;

                context.Movies.Add(dbMovie);
            }

            await context.SaveChangesAsync();
        }
    }
}
