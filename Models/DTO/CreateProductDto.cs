using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace storeInventoryApi.Models.DTO
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    


        public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
        {
            public CreateProductDtoValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Name cannot be empty");
                RuleFor(x => x.Price).GreaterThan(0);
            }
        }
    }


    
}
