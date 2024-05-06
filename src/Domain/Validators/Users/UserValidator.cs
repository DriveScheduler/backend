using Domain.Entities;
using Domain.Exceptions.Users;

using FluentValidation;

namespace Domain.Validators.Users
{
    public sealed class UserValidator : CustomValidator<User, UserValidationException>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Le nom est obligatoire");

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Le prénom est obligatoire");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("L'adresse email est obligatoire")
                .EmailAddress()
                .WithMessage("L'adresse email n'est pas valide");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Le mot de passe est obligatoire");
        }
    }
}
