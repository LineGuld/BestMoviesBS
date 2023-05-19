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
    
        public async Task<Toplist> GetToplist(string? userId)
        {
            return await UserDao.GetToplist(userId);
        } 
    }
}

