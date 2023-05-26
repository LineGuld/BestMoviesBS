using System.Collections.Generic;

namespace BestMoviesBS.Models
{
    public class Toplist
    {
        public string UserId { get; set; }
       // public int Id { get; set; }
        public List<int?> TitleIds = new List<int?>();
    }
}