using FluentValidation;

namespace storeInventoryApi.Models.DTO
{

    public class LoginUserRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }


        public class LoginUserRequestDtoValidator : AbstractValidator<LoginUserRequestDto>
        {
            public LoginUserRequestDtoValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("A valid email address is required.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            }
        }
    }


}
