using System.Threading.Tasks;
using BestMoviesBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestMoviesBS.Services
{
   public interface IUserService
   {
       Task<Toplist> GetToplist(string? userId);
       Task<Toplist> AddMovieToToplist(string userId, int tmdbId, int toplistNumber);
      // Task<Toplist> AddMovieToToplist(string userId, int tmdbId);
       Task<User> FindUser(string? userId);
       Task<User> AddUser(string userid, string username);
       Task<DeleteResult> DeleteUser(string? userId);
   } 
}

