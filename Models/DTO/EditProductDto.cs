using Microsoft.AspNetCore.Mvc;

namespace storeInventoryApi.Models.DTO
{
    public class EditProductDto
    {

        public Guid Id { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
