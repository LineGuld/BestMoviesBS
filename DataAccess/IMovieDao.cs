using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess;

public interface IMovieDao
{
    public Task<Movie> GetMovie(int? tmbdid);
    public Task<Movie> PutMovie(Movie movie);
}