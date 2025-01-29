using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Services;

namespace MovieReviewsAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        /// <summary>
        /// Create new movie
        /// </summary>
        /// <param name="dto">Movie details</param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST /api/movies
        ///     {
        ///         "title": "Top Gun",
        ///         "description": "Some description",
        ///         "directorFirstName": "Toni",
        ///         "directorLastName": "Scott",
        ///         "dateOfPremiere": "1986-05-12"
        ///     }
        /// </remarks>
        /// <response code="201">Returns Id of created movie</response>
        /// <response code="401">Not authorized</response>
        /// <response code="400">Something is wrong with given data</response>
        /// <returns></returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var movieId = await _movieService.CreateMovie(dto);

            return Created($"api/movies/{movieId}", movieId);
        }
        /// <summary>
        /// Get all movies in database
        /// </summary>
        /// <response code="200">Returns array of movies</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAll();

            return Ok(movies);
        }

        /// <summary>
        /// Get movie with given Id
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <response code="200">Returns info about the movie</response>
        /// <response code="404">Movie with given Id does not exist</response>
        /// <returns></returns>
        [HttpGet("{movieId}")]        
        public async Task<IActionResult> GetMovieById([FromRoute] int movieId)
        {
            var movie = await _movieService.GetById(movieId);

            return Ok(movie);
        }


        /// <summary>
        /// Update date of premiere
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     PUT /api/movies/{movieId}
        ///     {
        ///         "dateOfPremiere": "2023-01-11"
        ///     }
        /// 
        /// </remarks>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="dto"></param>
        /// <response code="200">Returns movie with updated date</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Movie with given Id does not exist</response>
        /// <response code="400">Something is wrong with given data</response>
        /// <returns></returns>
        [HttpPut("{movieId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UpdateDateOfPremiere([FromRoute] int movieId, [FromBody] UpdateDateOfPremiereDto dto)
        {
            var movie = await _movieService.UpdateDateOfPremiere(movieId, dto);

            return Ok(movie);
        }
    }
}
