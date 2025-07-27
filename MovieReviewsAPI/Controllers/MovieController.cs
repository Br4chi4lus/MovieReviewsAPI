using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

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
        /// <response code="200">
        ///     Returns array of movies with given length
        ///     Number of total pages
        ///     Number of total movies in DB
        ///     Index of first element(starting with 1)
        ///     Index of last element
        /// </response>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMovies([FromQuery] PaginationQuery movieQuery)
        {
            var result = await _movieService.GetAll(movieQuery);

            return Ok(result);
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
        ///         "dateOfPremiere": "2023-01-11",
        ///         "description": "Movie about..."
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
        public async Task<IActionResult> UpdateMovie([FromRoute] int movieId, [FromBody] UpdateMovieDto dto)
        {
            var movie = await _movieService.UpdateMovie(movieId, dto);

            return Ok(movie);
        }

        /// <summary>
        /// Update date of premiere
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     Delete /api/movies/{movieId}
        /// 
        /// </remarks>
        /// <param name="movieId">Id of the movie</param>
        /// <response code="204">No Content, movie deleted succesfully</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Movie with given Id does not exist</response>
        /// <returns></returns>
        [HttpDelete("{movieId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int movieId)
        {
            await _movieService.DeleteMovie(movieId);
            return NoContent();
        }
    }
}
