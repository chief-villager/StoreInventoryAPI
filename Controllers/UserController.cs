using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeInventoryApi.Service;

namespace storeInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDbContext _dbContext;
        public UserController(
            IUserService userService,
            IHttpContextAccessor httpContext,
            ApplicationDbContext dbContext
        )
        {
            _userService = userService;
            _httpContext = httpContext;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult> RegisterUserAsync(string Email, string UserName, string Role, string Password, CancellationToken cancellationToken)
        {
            await _userService.CreateAsync(Email, UserName, Role, Password, cancellationToken);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{UserEmail}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] string UserEmail, CancellationToken cancellationToken)
        {
            var UserId = _httpContext.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException("userId not found");
            await _userService.DeleteUser(UserId, UserEmail, cancellationToken);
            return Ok();

        }

        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ActionResult> LoginUserAsync(string Email, string Password, CancellationToken cancellationToken)
        {
            var response = await _userService.LoginUser(Email, Password, cancellationToken);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }


    }
}