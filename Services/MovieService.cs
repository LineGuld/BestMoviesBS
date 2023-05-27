using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;

namespace BestMoviesBS.Services;

public class MovieService: IMovieService
{
    private IMovieDao _movieDao;

    public MovieService(IMovieDao movieDao)
    {
        _movieDao = movieDao;
    }

    public async Task<Movie> GetMovie(int? tmbdid)
    {

        return await _movieDao.GetMovie(tmbdid);
    }

    public async Task<Movie> PutMovie(Movie movie)
    {
        return await _movieDao.PutMovie(movie);
    }
}