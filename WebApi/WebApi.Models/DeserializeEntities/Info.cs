using System;
using System.Collections.Generic;

namespace WebApi.Models.DeserializeEntities
{
    public class Info
    {
        public List<string> directors { get; set; }
        public DateTime release_date { get; set; }
        public double rating { get; set; }
        public List<string> genres { get; set; }
        public string image_url { get; set; }
        public string plot { get; set; }
        public int rank { get; set; }
        public int running_time_secs { get; set; }
        public List<string> actors { get; set; }
    }
}
