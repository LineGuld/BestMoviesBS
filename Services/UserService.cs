using System.Threading.Tasks;
using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;
using String = System.String;

namespace BestMoviesBS.Services
{
    public class UserService : IUserService
    {
        private IUserDao _userDao;

        public UserService(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public async Task<User> FindUser(string? userId)
        {
            return await _userDao.FindUser(userId);
        }

        public async Task<User> AddUser(string userid, string username)
        {
            User user = await _userDao.FindUser(userid);
            if (String.IsNullOrEmpty(user.Id) & String.IsNullOrEmpty(user.Username))
            {
                user.Id = userid;
                user.Username = username;
                return await _userDao.AddUser(user);
            }

            return user;
        }

        public async Task<DeleteResult> DeleteUser(string? userId)
        {
            return await _userDao.DeleteUser(userId);
        }


        public async Task<Toplist> GetToplist(string? userId)
        {
            Toplist toplist = await _userDao.GetToplist(userId);
            toplist.trimToplist();
            return toplist;
        }

        public async Task<Toplist> AddMovieToToplist(string userId, int tmdbId, int toplistNumber)
        {
            Toplist toplist = await _userDao.GetToplist(userId);

            toplist.TitleIds.Insert(toplistNumber - 1, tmdbId);

            toplist.trimToplist();

            await _userDao.SetToplist(userId, toplist);

            toplist = await _userDao.GetToplist(userId);
            toplist.trimToplist();
            return toplist;
        }
    }
}