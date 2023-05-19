using System.Threading.Tasks;
using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess
{
    public interface IUserDao
    {
        Task<Toplist> GetToplist(string userId);
    }
}

