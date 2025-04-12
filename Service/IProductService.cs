using storeInventoryApi.Models;
using storeInventoryApi.Models.DTO;

namespace storeInventoryApi.Service
{
    public interface IProductService
    {
        Task<ApiResponse<Products>> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteProductAsync(Guid ProductId, CancellationToken cancellationToken);
        Task<ApiResponse<Products>> EditProductDetailAsync(EditProductDto editProductDto, CancellationToken cancellationToken);
        Task<List<Products>> SearchProduct(string searchWord, CancellationToken cancellationToken);
        Task<ApiResponse<Products>> GetProduct(Guid ProductId, CancellationToken cancellationToken);

    }
}