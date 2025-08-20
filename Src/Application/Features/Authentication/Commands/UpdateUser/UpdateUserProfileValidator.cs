using FluentValidation;

namespace Application.Features.Authentication.Commands.UpdateUser
{
     public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.UpdateDto.Firstname).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.UpdateDto.Lastname).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.UpdateDto.Username).NotEmpty().WithMessage("Username is required.");
        }
    }
}