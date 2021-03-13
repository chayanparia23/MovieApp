namespace WebApi.Models.DeserializeEntities
{
    public class Movie
    {
        public int id { get; set; }
        public int year { get; set; }
        public string title { get; set; }
        public Info info { get; set; }
    }
}
