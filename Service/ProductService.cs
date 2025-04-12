using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using storeInventoryApi.Models;
using storeInventoryApi.Models.DTO;
using storeInventoryApi.Repository;

namespace storeInventoryApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Products> _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly TokenService _tokenService;
        public ProductService(
            IRepository<Products> productRepository,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            TokenService tokenService
        )
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<ApiResponse<Products>> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            if (createProductDto == null)
            {
                return new ApiResponse<Products>($"{nameof(createProductDto)} is null ",  isSuccess:false);
            }
            var userId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<Products>(" User is not Authenticated! ", isSuccess: false);
            }
            await _userManager.FindByIdAsync(userId);
            var verifyUser = await _userManager.FindByIdAsync(userId);
            if (verifyUser != null)
            {
                return new ApiResponse<Products>(" User is not Found! ", isSuccess: false);
            }
            ;
            var newProduct = createProductDto.Adapt<Products>();
            await _productRepository.CreateAsync(newProduct, cancellationToken);
            return new ApiResponse<Products>(newProduct);
        }

        public async Task<ApiResponse<string>> DeleteProductAsync(Guid ProductId, CancellationToken cancellationToken)
        {
            if (ProductId == Guid.Empty)
            {
                return new ApiResponse<string>("productId cannot be empty",false);
            }
            var userId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<string>("User is not authenticated.",false);
            }
            var verifyUser = await _userManager.FindByIdAsync(userId);
            if (verifyUser != null)
            {
                return new ApiResponse<string>(" User is not Found! ", false);
            }

            await _productRepository.DeleteAsync(ProductId, cancellationToken);
            return new ApiResponse<string>( message:"product deleted successfully",isSuccess:true);


        }

        public async Task<ApiResponse<Products>> EditProductDetailAsync(EditProductDto editProductDto, CancellationToken cancellationToken)
        {
            if (editProductDto == null)
            {
                return new ApiResponse<Products>($"{nameof(editProductDto)} cannot be null", isSuccess: false);
            }
            var userId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<Products>("User is not authenticated.", isSuccess: false);
            }
            var verifyUser = await _userManager.FindByIdAsync(userId);
            if (verifyUser != null)
            {
                return new ApiResponse<Products>(" User is not Found! ", isSuccess: false);
            }

            var existingProduct = await _productRepository.GetAsync(editProductDto.Id, cancellationToken);


            if (existingProduct == null)
            {
                return new ApiResponse<Products>(" Product is not Found! ", isSuccess: false);
            }

            editProductDto.Adapt(existingProduct);

            await _productRepository.UpdateAsync(existingProduct, cancellationToken);
            return new ApiResponse<Products>(existingProduct);
        }

        public async Task<ApiResponse<Products>> GetProduct(Guid ProductId, CancellationToken cancellationToken)
        {
            if (ProductId == Guid.Empty)
            {
                return new ApiResponse<Products>(" ProductId cannot be empty ", isSuccess: false);
            }
            var userId = _tokenService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<Products>(" UserId is not Found! ", isSuccess: false);
            }
            var verifyUser = await _userManager.FindByIdAsync(userId);
            if (verifyUser != null)
            {
                return new ApiResponse<Products>(" User is not Found! ", isSuccess: false);
            }
            var existingProduct = await _productRepository.GetAsync(ProductId, cancellationToken);
            if (existingProduct == null)
            {
                return new ApiResponse<Products>(" Product is not Found! ",false);
            }

            return new ApiResponse<Products>(existingProduct);
        }

        public async Task<List<Products>> SearchProduct(string searchWord, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                return new List<Products>();
            }
            return await _dbContext.Products
             .Where(p => p.Name.Contains(searchWord))
             .ToListAsync(cancellationToken);
        }
    }
}