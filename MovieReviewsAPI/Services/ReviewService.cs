using AutoMapper;
using CalendarAPI.Services;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieReviewsAPI.Authorization;

namespace MovieReviewsAPI.Services
{
    public class ReviewService : IReviewService
    {
        private MovieReviewsDbContext _dbContext;
        private IMapper _mapper;
        private IUserContextService _userContextService;
        private IAuthorizationService _authorizationService;
        public ReviewService(MovieReviewsDbContext dbContext, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public async Task<ReviewDto> CreateReview(int movieId, CreateReviewDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            var userId = _userContextService.GetUserId;
            if (userId is null)
                throw new UnauthorizedException("You are not logged in");
            review.UserId = (int)userId;
            review.MovieId = movieId;
            var date = DateTime.Now.ToUniversalTime();
            review.DateOfCreation = date;
            review.DateOfLastModification = date;

            await _dbContext.Review.AddAsync(review);
            await _dbContext.SaveChangesAsync();

            review = await _dbContext.Review
                .Include(r => r.Author)
                .FirstOrDefaultAsync(r => r.Id == review.Id);
            var reviewDto = _mapper.Map<ReviewDto>(review);
            
            return reviewDto;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviews(int movieId)
        {
            var movie = await _dbContext.Movie.FirstOrDefaultAsync(r => r.Id == movieId);
            if (movie is null)
                throw new NotFoundException("Movie with given id was not found");

            var reviews = await _dbContext.Review
                .Include(r => r.Author)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

            return reviewDtos;
        }

        public async Task<ReviewDto> UpdateReviewContent(int id, UpdateReviewContentDto dto)
        {
            var review = await _dbContext.Review
                .Include(r => r.Author)
                .FirstOrDefaultAsync (r => r.Id == id);
            if (review is null)
                throw new NotFoundException("Review does not exist");
            var user = _userContextService.User;
            if (user is null)
                throw new UnauthorizedException("You are not logged in");
            var authorizationResult = _authorizationService.AuthorizeAsync(user, review, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidenException();

            review.Content = dto.Content;
            await _dbContext.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return reviewDto;
        }

        public async Task DeleteReview(int id)
        {
            var review = await _dbContext.Review
                .Include(r => r.Author)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review is null)
                throw new NotFoundException("Review does not exist");
            var user = _userContextService.User;
            if (user is null)
                throw new UnauthorizedException("You are not logged in");
            var authorizationResult = _authorizationService.AuthorizeAsync(user, review, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidenException();

            _dbContext.Review.Remove(review);
            await _dbContext.SaveChangesAsync();
        }
    }

    public interface IReviewService
    {
        Task<ReviewDto> CreateReview(int movieId, CreateReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetAllReviews(int movieId);
        Task<ReviewDto> UpdateReviewContent(int id, UpdateReviewContentDto dto);
        Task DeleteReview(int id);
    }
}
