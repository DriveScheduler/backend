using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record CreateUser_Command(string Name, string Firstname, string Email, string Password, LicenceType LicenceType, UserType Type) : IRequest<Guid>;

    internal sealed class CreateUser_CommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUser_Command, Guid>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<Guid> Handle(CreateUser_Command request, CancellationToken cancellationToken)
        {
            if (_userRepository.IsEmailUnique(request.Email) == false)
                throw new UserValidationException("L'adresse email est déjà utilisée");

            User user = new User(
                request.Name,
                request.Firstname,
                request.Email,
                request.Password,
                request.LicenceType,
                request.Type);

            _userRepository.Insert(user);
            return Task.FromResult(user.Id);
        }
    }
}
