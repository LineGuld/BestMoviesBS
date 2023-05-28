using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;
using String = System.String;

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

        public async Task<User> AddUser(string userid, string username)
        {
            User user = await UserDao.FindUser(userid);
            if (String.IsNullOrEmpty(user.Id) & String.IsNullOrEmpty(user.Username))
            {
                user.Id = userid;
                user.Username = username;
                return await UserDao.AddUser(user);
            }

            return user;
        }

        public async Task<DeleteResult> DeleteUser(string? userId)
        {
            return await UserDao.DeleteUser(userId);
        }


        public async Task<Toplist> GetToplist(string? userId)
        {
            Toplist toplist = await UserDao.GetToplist(userId);
            toplist.trimToplist();
            return toplist;
        }

        public async Task<Toplist> AddMovieToToplist(string userId, int tmdbId, int toplistNumber)
        {
            Toplist toplist = await UserDao.GetToplist(userId);

            toplist.TitleIds.Insert(toplistNumber - 1, tmdbId);

            toplist.trimToplist();

            await UserDao.SetToplist(userId, toplist);

            return await UserDao.GetToplist(userId);
        }
    }
}