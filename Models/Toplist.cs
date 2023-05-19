namespace BestMoviesBS.Models
{
    public class Toplist
    {
        public int Id { get; set; }
        public List<int?> TitleIds = new List<int?>();
    }
}