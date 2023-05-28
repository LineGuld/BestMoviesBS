using System.Threading.Tasks;
using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess
{
    public interface IMovieDao
    {
        public Task<Movie> GetMovie(int? tmbdid);
        public Task<Movie> PutMovie(Movie movie);
        Task<Movie> SetTitle(Movie movie);
    } 
}

