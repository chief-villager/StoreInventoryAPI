using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeInventoryApi.Models;
using storeInventoryApi.Models.DTO;

namespace storeInventoryApi.Service
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponseDto>> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken);
        Task<ApiResponse<string>> LoginUser(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken);
        Task<ApiResponse<UserResponseDto>> GetUser(CancellationToken cancellationToken);
        Task<ApiResponse<UserResponseDto>> GetUser(string userId, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteUser(string UserId, CancellationToken cancellationToken);

    }
}