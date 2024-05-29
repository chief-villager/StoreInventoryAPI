using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeInventoryApi.Models;
using storeInventoryApi.Service;

namespace storeInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(
             IProductService productService,
             IHttpContextAccessor httpContextAccessor

        )
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateProductAsync(string Name, decimal Price, CancellationToken cancellationToken)
        {
            var UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                            ?? throw new NullReferenceException("userId not found");
            await _productService.CreateProductAsync(Name, UserId, Price, cancellationToken);
            return Ok();

        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("edit/{ProductId:Guid}")]
        public async Task<ActionResult> EditProductAsync(Guid ProductId, string Name, decimal Price, CancellationToken cancellationToken)
        {
            var UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                                        ?? throw new NullReferenceException("userId not found");
            await _productService.EditProductDetailAsync(ProductId, UserId, Name, Price, cancellationToken);
            return Ok();
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("delete/{ProductId:Guid}")]
        public async Task<ActionResult> DeleteProductAsync(Guid ProductId, CancellationToken cancellationToken)
        {
            var UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                                                    ?? throw new NullReferenceException("userId not found");

            await _productService.DeleteProductAsync(ProductId, UserId, cancellationToken);
            return Ok();

        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<List<Products>>> SearchProductAsync(string query, CancellationToken cancellationToken)
        {
            var products = await _productService.SearchProduct(query, cancellationToken);
            return Ok(products);
        }
    }
}