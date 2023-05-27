using BestMoviesBS.Models;

namespace BestMoviesBS.Services;

public interface IMovieService
{
    public Task<Movie> GetMovie(int? tmbdid);
    public Task<Movie> PutMovie(Movie movie);
}