using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using storeInventoryApi.Models;
using storeInventoryApi.Repository;

namespace storeInventoryApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Products> _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(
            IRepository<Products> productRepository,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task CreateProductAsync(string Name, string UserId, decimal Price, CancellationToken cancellationToken)
        {
            if (Name == null || UserId == null || Price <= 0 || Guid.Parse(UserId) == Guid.Empty)
            {
                throw new NullReferenceException($"{Name} or {UserId} or {Price} cannot be null");
            };
            _ = await _userManager.FindByIdAsync(UserId) ?? throw new NullReferenceException("User not found");
            Products product = new()
            {
                Name = Name,
                Price = Price,

            };
            await _productRepository.CreateAsync(product, cancellationToken);
        }

        public async Task DeleteProductAsync(Guid ProductId, string UserId, CancellationToken cancellationToken)
        {
            if (ProductId == Guid.Empty || Guid.Parse(UserId) == Guid.Empty || String.IsNullOrEmpty(UserId))
            {
                throw new ArgumentNullException($"{nameof(ProductId)},{nameof(UserId)} cannot be null .");
            }
            _ = await _userManager.FindByIdAsync(UserId)
                     ?? throw new NullReferenceException("User not found");
            await _productRepository.DeleteAsync(ProductId, cancellationToken);

        }

        public async Task EditProductDetailAsync(Guid ProductId, string UserId, string productName, decimal Price, CancellationToken cancellationToken)
        {
            if (ProductId == Guid.Empty || Guid.Parse(UserId) == Guid.Empty || String.IsNullOrEmpty(UserId) || String.IsNullOrEmpty(productName) || Price <= 0)
            {
                throw new ArgumentNullException($"{nameof(ProductId)},{nameof(UserId)},{nameof(Price)},{nameof(productName)} cannot be null .");
            }
            _ = await _userManager.FindByIdAsync(UserId)
                    ?? throw new NullReferenceException("User not found");

            var existingProduct = await _productRepository.GetAsync(ProductId, cancellationToken)
                             ?? throw new NullReferenceException("product not found");
            existingProduct.Name = productName;
            existingProduct.Price = Price;
            await _productRepository.UpdateAsync(existingProduct, cancellationToken);
        }

        public async Task<List<Products>> SearchProduct(string searchWord, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                return new List<Products>();
            }
            return await _dbContext.Products.Where(p => p.Name != null && p.Name.Contains(searchWord)).ToListAsync(cancellationToken);
        }
    }
}