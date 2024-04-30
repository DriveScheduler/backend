using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record UpdateUser_Command(Guid UserId, string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest;

    internal sealed class UpdateUser_RequestHandler(IDatabase database) : IRequestHandler<UpdateUser_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(UpdateUser_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            User model = new User()
            {
                Id = request.UserId,
                Name = request.Name,
                Firstname = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType
            };

            new UserValidator().ThrowIfInvalid(model);

            user.Name = model.Name;
            user.Firstname = model.Firstname;
            user.Email = model.Email;
            user.LicenceType = model.LicenceType;

            if (await _database.SaveChangesAsync(cancellationToken) != 1)
                throw new UserSaveException();
        }
    }
}
