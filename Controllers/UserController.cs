using BestMoviesBS.Models;
using BestMoviesBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestMoviesBS.Controllers;

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
        [Route("toplist/{userId}")]
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
}