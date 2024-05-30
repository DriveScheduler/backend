using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record UpdateUser_Command(Guid UserId, string Name, string Firstname, string Email) : IRequest;

    internal sealed class UpdateUser_CommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUser_Command>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task Handle(UpdateUser_Command request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);

            if (user.Email.Value != request.Email && _userRepository.IsEmailUnique(request.Email) == false)
                throw new UserValidationException("L'adresse email est déjà utilisée");

            user.Update(request.Name, request.Firstname, request.Email);

            _userRepository.Update(user);
            return Task.CompletedTask;
        }
    }
}
