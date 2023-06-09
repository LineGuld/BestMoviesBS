﻿using System.Threading.Tasks;
using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess
{
    public interface IUserDao
    {
        Task<Toplist> GetToplist(string userId);
        Task SetToplist(string userId, Toplist toplist);
        Task<User> FindUser(string userID);
        Task<User> AddUser(User user);
        Task<DeleteResult> DeleteUser(string? userId);
    }
}

