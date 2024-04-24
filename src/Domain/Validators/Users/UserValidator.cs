using Domain.Entities;
using Domain.Exceptions.Users;

using FluentValidation;

namespace Domain.Validators.Users
{
    public sealed class UserValidator : AbstractValidator<User>
    {
        private UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Le nom est obligatoire");

            RuleFor(user => user.Firstname)
                .NotEmpty()
                .WithMessage("Le prénom est obligatoire");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("L'adresse email est obligatoire")
                .EmailAddress()
                .WithMessage("L'adresse email n'est pas valide");
        }


        public static void ThrowIfInvalid(User user)
        {
            var results = new UserValidator().Validate(user);
            if (!results.IsValid)
            {
                throw new UserValidationException(results.Errors[0].ErrorMessage);
            }
        }
    }
}
