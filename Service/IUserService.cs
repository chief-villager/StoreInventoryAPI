using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeInventoryApi.Models;

namespace storeInventoryApi.Service
{
    public interface IUserService
    {
        Task CreateAsync(string Email, string UserName, string Role, string Password, CancellationToken cancellationToken);
        Task<bool> LoginUser(string Email, string Password, CancellationToken cancellationToken);
        Task<ApplicationUser> GetUser(string UserId, string UserEmail, CancellationToken cancellationToken);
        Task DeleteUser(string UserId, string UserEmail, CancellationToken cancellationToken);

    }
}