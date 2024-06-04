using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;
using Domain.Exceptions.Users;

namespace Application.UseCases.Users.Commands
{
    public sealed record DeleteUser_Command(Guid id) : IRequest;

    internal sealed class DeleteUser_CommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUser_Command>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task Handle(DeleteUser_Command request, CancellationToken cancellationToken)
        {           
            _userRepository.DeleteById(request.id);
            return Task.CompletedTask;
        }
    }
}