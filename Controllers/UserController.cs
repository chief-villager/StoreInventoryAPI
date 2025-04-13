using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeInventoryApi.Models.DTO;
using storeInventoryApi.Service;

namespace storeInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(
            IUserService userService
        )
        {
            _userService = userService;
           
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateAsync(request, cancellationToken);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
        {
            var response = await _userService.DeleteUser(userId, cancellationToken);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _userService.LoginUser(request, cancellationToken);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var response = await _userService.GetUser(cancellationToken);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [Authorize(Roles ="Admin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId, CancellationToken cancellationToken)
        {
            var response = await _userService.GetUser(userId, cancellationToken);
            return StatusCode(response.Success ? 200 : 400, response);
        }
    }
}