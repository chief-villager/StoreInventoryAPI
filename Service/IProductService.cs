using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeInventoryApi.Models;

namespace storeInventoryApi.Service
{
    public interface IProductService
    {
        Task CreateProductAsync(string Name, string UserId, Decimal Price, CancellationToken cancellationToken);
        Task DeleteProductAsync(Guid ProductId, string UserId, CancellationToken cancellationToken);
        Task EditProductDetailAsync(Guid ProductId, string UserId, string productName, decimal Price, CancellationToken cancellationToken);
        Task<List<Products>> SearchProduct(string searchWord, CancellationToken cancellationToken);

    }
}