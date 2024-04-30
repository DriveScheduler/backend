using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record CreateUser_Command(string Name, string Firstname, string Email, LicenceType LicenceType, UserType UserType) : IRequest<Guid>;

    internal sealed class CreateUser_RequestHandler(IDatabase database) : IRequestHandler<CreateUser_Command, Guid>
    {
        private readonly IDatabase _database = database;

        public async Task<Guid> Handle(CreateUser_Command request, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                Name = request.Name,
                Firstname = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType,
                UserType = request.UserType
            };

            new UserValidator().ThrowIfInvalid(user);

            _database.Users.Add(user);
            if (await _database.SaveChangesAsync() != 1)
            {
                throw new UserSaveException();
            }

            return user.Id;
        }
    }
}
