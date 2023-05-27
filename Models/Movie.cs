using System.ComponentModel.DataAnnotations;

namespace BestMoviesBS.Models;

public class Movie
{
    public int? Tmdbid { get; set; }
    public string? Title { get; set; }
}