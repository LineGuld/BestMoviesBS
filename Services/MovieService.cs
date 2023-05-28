using System;
using System.Threading.Tasks;
using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;

namespace BestMoviesBS.Services
{
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
            Movie exists = await _movieDao.GetMovie(movie.Tmdbid);
            if (exists.Tmdbid == null)
            {
                return await _movieDao.PutMovie(movie);
            }
            else if (String.IsNullOrEmpty(exists.Title) && String.IsNullOrEmpty(movie.Title)==false)
            {
                return await _movieDao.SetTitle(movie);
            }
            else
            {
                return exists;
            }
        }
    }
}

