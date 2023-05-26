using System.Collections.Generic;

namespace BestMoviesBS.Models
{
    public class Toplist
    {
        public string UserId { get; }
       // public int Id { get; set; }
       public List<int?> TitleIds;

        public Toplist(string userId)
        {
            UserId = userId;
            TitleIds = new List<int?>();
            for (int i = 0; i < 5; i++)
            {
                TitleIds.Add(null);
            }
        }

        public void trimToplist()
        {
            if (TitleIds.Count > 5)
            {
                int excess = TitleIds.Count - 5; 
                TitleIds.RemoveRange(5, excess);
            }
        }
    }
}