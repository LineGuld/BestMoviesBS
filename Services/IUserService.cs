using BestMoviesBS.Models;

namespace BestMoviesBS.Services;

public interface IUserService
{
    Task<Toplist> GetToplist(string? userId); 
}