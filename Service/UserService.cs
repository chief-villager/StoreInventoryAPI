using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using storeInventoryApi.CustomExeptions;
using storeInventoryApi.Enum;
using storeInventoryApi.Models;

namespace storeInventoryApi.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<string>> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor contextAccessor,
            ApplicationDbContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task CreateAsync(string Email, string UserName, string Role, string Password, CancellationToken cancellationToken)
        {
            if (Email == null || UserName == null || Password == null)
            {
                throw new ArgumentNullException("Email,UserName or Password cannot be null");
            }
            var existinguser = await _userManager.FindByEmailAsync(Email);
            if (existinguser != null)
            {
                throw new UserAlreadyExistsException($"user with the {Email} already exist");
            }
            ApplicationUser user = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = Email,
                UserName = UserName
            };
            var result = await _userManager.CreateAsync(user, Password);
            var newUser = result.Succeeded
                    ? await _userManager.FindByEmailAsync(Email)
                    ?? throw new NullReferenceException("")
                    : throw new NullReferenceException("");


            if (!await _roleManager.RoleExistsAsync(Role))
            {
                throw new NullReferenceException("Role not available");
            }
            await _userManager.AddToRoleAsync(newUser, Role);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteUser(string UserId, string UserEmail, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(UserId) || Guid.Parse(UserId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(UserId));
            }
            var user = await _userManager.FindByEmailAsync(UserEmail) ?? throw new NullReferenceException("user not found");
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ApplicationUser> GetUser(string UserId, string UserEmail, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(UserId) || Guid.Parse(UserId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(UserId));
            }
            _ = await _userManager.FindByIdAsync(UserId.ToString()) ?? throw new NullReferenceException("user not found");
            return await _userManager.FindByEmailAsync(UserEmail) ?? throw new NullReferenceException("user not found");


        }

        public async Task<bool> LoginUser(string Email, string Password, CancellationToken cancellationToken)
        {
            try
            {
                if (Email == null || Password == null)
                {
                    throw new ArgumentNullException("Email,UserName or Password cannot be null");
                }
                var user = await _userManager.FindByEmailAsync(Email) ?? throw new NullReferenceException("user not found");
                var userRole = await _userManager.GetRolesAsync(user);
                var verifyPassword = await _userManager.CheckPasswordAsync(user, Password);
                if (!verifyPassword)
                {
                    throw new ApplicationException("password verification failed");
                }
                var claims = new List<Claim>
                {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Role, user.Email!)

                };
                claims.AddRange(userRole.Select(role => new Claim(ClaimTypes.Role, role)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = true,
                    AllowRefresh = true
                };
                await _contextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
                return true;
            }
            catch
            {

                return false;
            }



        }

    }


}

