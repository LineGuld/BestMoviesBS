using System.Threading.Tasks;
using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;

namespace BestMoviesBS.Services
{
    public class UserService : IUserService
    {
        private IUserDao UserDao;

        public UserService(IUserDao userDao)
        {
            UserDao = userDao;
        }

        public async Task<User> FindUser(string? userId)
        {
            return await UserDao.FindUser(userId);
        }


        public async Task<Toplist> GetToplist(string? userId)
        {
            return await UserDao.GetToplist(userId);
        }

        public async Task<Toplist> AddMovieToToplist(string userId, int tmdbId)
        {
            if (tmdbId != null)
            {
                return await UserDao.AddMovieToToplist(userId, tmdbId);
            }
            else
            {
                return await UserDao.GetToplist(userId);;
            }
        }
    }
}