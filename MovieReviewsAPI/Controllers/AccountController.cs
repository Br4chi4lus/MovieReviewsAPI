using Microsoft.AspNetCore.Mvc;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Services;

namespace MovieReviewsAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        /// <summary>
        /// Creates new User
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST /api/account/register
        ///     {
        ///         "email": "test@test.com",
        ///         "password" : "password123",
        ///         "passwordConfirm": "password123",
        ///         "userName": "User1",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "dateOfBirth": "2000-11-11"
        ///     }
        /// </remarks>
        /// <response code="201">Returns Id of created user</response>
        /// <response code="400">Email taken or passwords do not match</response>
        ///
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            var userId = await _accountService.RegisterUser(dto);

            return Created($"api/account/{userId}", userId);
        }
        /// <summary>
        /// Logs in user
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST /api/account/login
        ///     {
        ///         "email": "test@test.com",
        ///         "password" : "password123"
        ///     }
        /// </remarks>
        /// <response code="200">User logged in</response>
        /// <response code="400">Invalid email or password</response>
        /// <returns>jwt token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            var token = await _accountService.LoginUser(dto);

            return Ok(token);
        }
    }
}
