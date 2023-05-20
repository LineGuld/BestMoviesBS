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

            [HttpPut]
            [Route("toplist/{userid}")]
            public async Task<ActionResult> AddMovieToToplist([FromRoute] string userid, [FromQuery] int tmdbId, [FromQuery] int toplistNumber)
            {
                try
                {
                   // new Toplist  = await UserService.AddMovieToToplist(userid, tmdbId,toplistNumber);
                    return Ok(await UserService.AddMovieToToplist(userid, tmdbId,toplistNumber));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, e.Message);
                }
            }
    }
}

   