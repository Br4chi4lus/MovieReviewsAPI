using Microsoft.AspNetCore.Identity;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
namespace MovieReviewsAPI.Services
{
    public class AccountService : IAccountService
    {
        private MovieReviewsDbContext _dbContext;
        private IPasswordHasher<User> _passwordHasher;
        private AuthenticationSettings _authenticationSettings;
        public AccountService(MovieReviewsDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<int> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto.Password != registerUserDto.PasswordConfirm)
                throw new BadRequestException("Passwords do not match");
            User user = new User()
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                DateOfBirth = registerUserDto.DateOfBirth,
                CreationDate = new DateTime(),
                UserName = registerUserDto.UserName,
                RoleId = 1,
            };

            var hashedPassword = _passwordHasher.HashPassword(user, registerUserDto.Password);
            user.PasswordHash = hashedPassword;
            await _dbContext.User.AddAsync(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public async Task<string> LoginUser(LoginUserDto loginUserDto)
        {
            var user = await _dbContext.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);
            if (user is null)
                throw new BadRequestException("Invalid email or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            if (user.DateOfBirth is not null)
            {
                claims.Add(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }
    }
    public interface IAccountService
    {
        Task<int> RegisterUser(RegisterUserDto registerUserDto);
        Task<string> LoginUser(LoginUserDto loginUserDto);
    }
}
