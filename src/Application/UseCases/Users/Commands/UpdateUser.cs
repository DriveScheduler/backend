using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record UpdateUser_Command(Guid UserId, string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest;

    internal sealed class UpdateUser_CommandHandler(IDatabase database) : IRequestHandler<UpdateUser_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(UpdateUser_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();          

            user.Name = request.Name;
            user.FirstName = request.Firstname;
            user.Email = request.Email;
            user.LicenceType = request.LicenceType;

            new UserValidator(_database).ThrowIfInvalid(user);

            if (await _database.SaveChangesAsync(cancellationToken) != 1)
                throw new UserSaveException();
        }
    }
}
