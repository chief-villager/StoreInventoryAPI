using FluentValidation;

namespace storeInventoryApi.Models.DTO
{


    public class CreateUserRequestDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        // Fluent Validator
        public class Validator : AbstractValidator<CreateUserRequestDto>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Email format is invalid.");

                RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("Username is required.")
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

                RuleFor(x => x.Role)
                    .NotEmpty().WithMessage("Role is required.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            }
        }
    }

}
