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

        public async Task<Toplist> AddMovieToToplist(string userId, int tmdbId, int toplistNumber)
        {
            Toplist toplist = await UserDao.GetToplist(userId);

            if (toplist.TitleIds[toplistNumber-1] == null)
            {
              toplist.TitleIds[toplistNumber-1] = tmdbId;  
            }
            else
            {
                toplist.TitleIds.Insert(toplistNumber-1, tmdbId);
                int excess = toplist.TitleIds.Count - 5;
                toplist.TitleIds.RemoveRange(5, excess);                
            }

            await UserDao.SetToplist(userId, toplist);
             
          toplist.TitleIds.Clear();
          return await UserDao.GetToplist(userId);
        }
    }
}

