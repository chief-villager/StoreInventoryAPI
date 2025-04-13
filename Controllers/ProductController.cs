using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeInventoryApi.Models.DTO;
using storeInventoryApi.Service;

namespace storeInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            var result = await _productService.CreateProductAsync(createProductDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{productId:guid}")]
        public async Task<IActionResult> EditProductAsync([FromRoute] Guid productId, [FromBody] EditProductDto editProductDto, CancellationToken cancellationToken)
        {
            editProductDto.Id = productId;

            var result = await _productService.EditProductDetailAsync(editProductDto, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var result = await _productService.DeleteProductAsync(productId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("search/{search}")]
        public async Task<IActionResult> SearchProductAsync([FromRoute] string search, CancellationToken cancellationToken)
        {
            var result = await _productService.SearchProduct(search, cancellationToken);

            if (result == null || result.Count == 0)
                return NotFound("No products found matching your search criteria.");

            return Ok(result);
        }
    }
}
