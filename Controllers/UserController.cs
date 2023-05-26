using System;
using System.Threading.Tasks;
using BestMoviesBS.Models;
using BestMoviesBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestMoviesBS.Controllers
{
     [ApiController]
        [Route("/user")]
        public class UserController : ControllerBase
        {
            private IUserService UserService;
        
            public UserController(IUserService userService)
            {
                this.UserService = userService;
            }
    
            [HttpGet]
            [Route("{userId}")]
            public async Task<ActionResult> GetUser([FromRoute] string? userId)
            {
                try
                {
                    User user = await UserService.FindUser(userId);
                    return Ok(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
            
            [HttpPut]
            [Route("")]
            public async Task<ActionResult> AddUser([FromQuery] string userid, [FromQuery] string username)
            {
                try
                {
                    return Ok(await UserService.AddUser(userid,username));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
            
            [HttpGet]
            [Route("{userId}/toplist")]
            public async Task<ActionResult> GetToplist([FromRoute] string? userId)
            {
                try
                {
                    Toplist toplist = await UserService.GetToplist(userId);
                    return Ok(toplist);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }

            [HttpPut]
            [Route("{userId}/toplist")]
            public async Task<ActionResult> AddMovieToToplist([FromRoute] string userid, [FromQuery] int tmdbId, [FromQuery] int number)
            {
                try
                {
                   // new Toplist  = await UserService.AddMovieToToplist(userid, tmdbId,toplistNumber);
                    return Ok(await UserService.AddMovieToToplist(userid, tmdbId, number));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
    }
}

   