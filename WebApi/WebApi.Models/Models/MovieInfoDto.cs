using System;
using System.Collections.Generic;

namespace WebApi.Models.Models
{
    public class MovieInfoDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public List<string> Directors { get; set; }
        public DateTime ReleasedDate { get; set; }
        public double Rating { get; set; }
        public List<string> Genres { get; set; }
        public string ImageUrl { get; set; }
        public string Plot { get; set; }
        public int Rank { get; set; }
        public int RunningTime { get; set; }
        public List<string> Actors { get; set; }
    }
}
