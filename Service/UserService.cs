using Mapster;
using Microsoft.AspNetCore.Identity;
using storeInventoryApi.Models;
using storeInventoryApi.Models.DTO;

namespace storeInventoryApi.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public UserService(
             UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager,
             TokenService tokenService,
             ApplicationDbContext applicationDbContext,
             SignInManager<ApplicationUser> signInManager

        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
        }

        public async Task<ApiResponse<UserResponseDto>> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken)
        {
            using var transaction = await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<UserResponseDto>("User with the provided email already exists.", false);
                }

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    //get the errors description from the identityResult and output it 
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ApiResponse<UserResponseDto>($"User creation failed: {errors}", false);
                }

                var newUser = await _userManager.FindByEmailAsync(request.Email);
                if (newUser == null)
                {
                    return new ApiResponse<UserResponseDto>("Newly created user not found.", false);
                }

                if (!await _roleManager.RoleExistsAsync(request.Role))
                {
                    return new ApiResponse<UserResponseDto>("Role not available.", false);
                }

                await _userManager.AddToRoleAsync(newUser, request.Role);

                var responseDto = newUser.Adapt<UserResponseDto>();
                responseDto.Role = request.Role;
                await transaction.CommitAsync(cancellationToken);
                return new ApiResponse<UserResponseDto>(responseDto);
               

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<UserResponseDto>($"An error occurred: {ex.Message}", false);
            };

        }




        public async Task<ApiResponse<string>> DeleteUser(string UserId, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(UserId))
            {
                throw new ArgumentNullException(nameof(UserId));
            }
            var requesterId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(requesterId))
            {
                return new ApiResponse<string>("User is not Authenticated", false);
            }
            var user = await _userManager.FindByIdAsync(requesterId);
            if (user == null)
            {

                return new ApiResponse<string>("User is not Found", false);
            }
            var userToDelete = await _userManager.FindByIdAsync(requesterId);
            if (userToDelete == null)
            {
                return new ApiResponse<string>("User is not Found", false);
            }
            var result = await _userManager.DeleteAsync(userToDelete);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ApiResponse<string>($"User creation failed: {errors}", false);
            }
            return new ApiResponse<string>("Operation successful", true);


        }

        public async Task<ApiResponse<UserResponseDto>> GetUser(CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<UserResponseDto>("User is not Authenticated", false);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<UserResponseDto>("User is not Found", false);
            }
            var userResponseDto = user.Adapt<UserResponseDto>();
            return new ApiResponse<UserResponseDto>(userResponseDto);


        }

        public async Task<ApiResponse<UserResponseDto>> GetUser(string userId, CancellationToken cancellationToken)
        {
            var requesterId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<UserResponseDto>("User is not Authenticated", false);
            }

            //check id of the person making the request . usually an admin with Authorization
            var checkRequesterId = await _userManager.FindByIdAsync(userId);
            if (checkRequesterId == null)
            {
                return new ApiResponse<UserResponseDto>("User is not Found", false);
            }
            // verify and get the id being user being requested for
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<UserResponseDto>("User is not Found", false);
            }
            var userResponseDto = user.Adapt<UserResponseDto>();
            return new ApiResponse<UserResponseDto>(userResponseDto);

        }


        public async Task<ApiResponse<string>> LoginUser(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken)
        {


            var user = await _userManager.FindByEmailAsync(loginUserRequestDto.Email);
            if (user == null)
            {
                return new ApiResponse<string>("User not found", false);
            }
            var userRole = await _userManager.GetRolesAsync(user);
            //using signInManager offers more control and security for sign in over userManager in .net identity
            var verifyPassword = await _signInManager.CheckPasswordSignInAsync(user, loginUserRequestDto.Password, false);

            if (!verifyPassword.Succeeded)
            {
                return new ApiResponse<string>("password verification failed", false);
            }
            var token = _tokenService.CreateToken(user, userRole);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<string>("invalid token", false);
            }

            return new ApiResponse<string>(token);

        }

    }


}

