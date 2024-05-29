using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeInventoryApi.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Guid Id, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task DeleteAsync(Guid Id, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);

    }
}