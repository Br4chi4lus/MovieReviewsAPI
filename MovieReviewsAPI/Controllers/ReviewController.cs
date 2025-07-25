using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Services;

namespace MovieReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId}/reviews")]
    public class ReviewController : ControllerBase
    {
        private IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Creates new review
        /// </summary>
        /// <param name="movieId">Id of the movie review is about</param>
        /// <param name="dto"></param>
        /// <response code="200">Return created review</response>
        /// <response code="401">Not authorized, user is not logged in</response>
        /// <response code="400">Something is wrong with data</response>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromRoute] int movieId, [FromBody] CreateReviewDto dto)
        {
            var review = await _reviewService.CreateReview(movieId, dto);

            return Ok(review);
        }

        /// <summary>
        /// Gets all reviews of the movie with given Id
        /// </summary>
        /// <param name="movieId"></param>
        /// <response code="200">Returns array of reviews with given movieId</response>
        /// <response code="404">Movie with given Id does not exist</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllReviews([FromRoute] int movieId, [FromQuery] PaginationQuery reviewQuery)
        {
            var reviews = await _reviewService.GetAllReviews(movieId, reviewQuery);

            return Ok(reviews);
        }

        /// <summary>
        /// Updates content of the review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="dto"></param>
        /// 
        /// <returns></returns>
        [HttpPut("{reviewId}")]
        [Authorize]
        public async Task<IActionResult> UpdateReviewContent([FromRoute] int reviewId, [FromBody] UpdateReviewContentDto dto)
        {
            var review = await _reviewService.UpdateReviewContent(reviewId, dto);

            return Ok(review);
        }

        [HttpDelete("{reviewId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromRoute] int reviewId)
        {
            await _reviewService.DeleteReview(reviewId);

            return NoContent();
        }
    }
}
