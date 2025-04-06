using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Exceptions;

namespace MovieReviewsAPI.Services
{
    public class MovieService : IMovieService
    {
        private MovieReviewsDbContext _dbContext;
        private IMapper _mapper;
        public MovieService(MovieReviewsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateMovie(CreateMovieDto dto)
        {
            var dir = _dbContext.Director.FirstOrDefault(d => d.FirstName == dto.DirectorFirstName && d.LastName == dto.DirectorLastName);
            if (dir is null)
            {
                dir = new Director
                {
                    FirstName = dto.DirectorFirstName,
                    LastName = dto.DirectorLastName,
                };
                await _dbContext.Director.AddAsync(dir);
                await _dbContext.SaveChangesAsync();
            }

            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                DirectorId = dir.Id,
                DateOfPremiere = (dto.DateOfPremiere is not null) ? dto.DateOfPremiere : null
            };

            await _dbContext.Movie.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            return movie.Id;
        }

        public async Task<IEnumerable<MovieDto>> GetAll()
        {
            var movies = await _dbContext.Movie
                .Include(m => m.Director)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    DateOfPremiere = (DateTime)m.DateOfPremiere,
                    Director = m.Director.FirstName + " " + m.Director.LastName,
                    AverageRating = m.Reviews.Average(r => (double?)r.Rating) ?? 0,
                })
                .ToListAsync();


            return movies;
        }

        public async Task<MovieDto> GetById(int id)
        {
            var movie = await _dbContext.Movie
                .Include(m => m.Director)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    DateOfPremiere = (DateTime)m.DateOfPremiere,
                    Director = m.Director.FirstName + " " + m.Director.LastName,
                    AverageRating = m.Reviews.Average(r => (double?)r.Rating) ?? 0,
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                throw new NotFoundException("Movie with given id was not found");

            return movie;
        }

        public async Task<MovieDto> UpdateDateOfPremiere(int id, UpdateDateOfPremiereDto dto)
        {
            var movie = await _dbContext.Movie
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                throw new NotFoundException("Movie with given id was not found");

            movie.DateOfPremiere = dto.DateOfPremiere;
            await _dbContext.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDto>(movie);
            return movieDto;
        }
        public async Task DeleteMovie(int id)
        {
            var movie = await _dbContext.Movie
                .FirstOrDefaultAsync (m => m.Id == id);

            if (movie is null)
                throw new NotFoundException("Movie with given id was not found");

            _dbContext.Movie.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }
    }

    public interface IMovieService
    {
        Task<int> CreateMovie(CreateMovieDto dto);
        Task<IEnumerable<MovieDto>> GetAll();
        Task<MovieDto> GetById(int id);
        Task<MovieDto> UpdateDateOfPremiere(int id, UpdateDateOfPremiereDto dto);
        Task DeleteMovie(int id);
    }
}
