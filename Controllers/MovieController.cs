using System;
using System.Threading.Tasks;
using BestMoviesBS.Models;
using BestMoviesBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestMoviesBS.Controllers
{
    [ApiController]
    [Route("/movies")]
    public class MovieController: ControllerBase
    {
        private IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        
        [HttpGet]
        [Route("{movieId}")]
        public async Task<ActionResult> GetMovie([FromRoute] int? movieId)
        {
            if (movieId == null)
            {
                return StatusCode(400, "MoiveId cannot be null");
            }
            try
            {
                Movie movie = await _movieService.GetMovie(movieId);
                return Ok(movie);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPut]
        public async Task<ActionResult> AddMovie([FromBody] Movie movie)
        {
            if (movie.Tmdbid == null)
            {
                return StatusCode(400, "MoiveId cannot be null");
            }
            try
            {
                return Ok(await _movieService.PutMovie(movie));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}