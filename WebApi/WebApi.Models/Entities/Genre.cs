namespace WebApi.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
    }
}
