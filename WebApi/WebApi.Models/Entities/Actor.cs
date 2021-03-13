namespace WebApi.Models.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
    }
}
