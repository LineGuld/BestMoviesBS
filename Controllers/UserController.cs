﻿using System;
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
            private IUserService _userService;
        
            public UserController(IUserService userService)
            {
                this._userService = userService;
            }
    
            [HttpGet]
            [Route("{userId}")]
            public async Task<ActionResult> GetUser([FromRoute] string? userId)
            {
                try
                {
                    User user = await _userService.FindUser(userId);
                    return Ok(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
            
            [HttpPut]
    
            public async Task<ActionResult> AddUser([FromQuery] string userId, [FromQuery] string username)
            {
                try
                {
                    return Ok(await _userService.AddUser(userId,username));
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
                    Toplist toplist = await _userService.GetToplist(userId);
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
            public async Task<ActionResult> AddMovieToToplist([FromRoute] string userId, [FromQuery] int tmdbId, [FromQuery] int number)
            {
                try
                {
                   // new Toplist  = await UserService.AddMovieToToplist(userid, tmdbId,toplistNumber);
                    return Ok(await _userService.AddMovieToToplist(userId, tmdbId, number));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
    }
}

   