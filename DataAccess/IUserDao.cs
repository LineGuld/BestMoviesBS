using System.Threading.Tasks;
using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess
{
    public interface IUserDao
    {
        Task<Toplist> GetToplist(string userId);
        Task SetToplist(string userId, Toplist toplist);
        Task<User> NewUserCheck(string userId);
        Task AddNewUser(string userId, string userName);
    }
}

