using Domain.Abstractions;
using Domain.Entities;
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

            if (_database.Users.FirstOrDefault(u => u.Email == request.Email) is not null)
                throw new UserValidationException("L'adresse email est déjà utilisée");

            User model = new User()
            {
                Id = request.UserId,
                Name = request.Name,
                FirstName = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType,
                Password = user.Password,
                Type = user.Type
            };

            new UserValidator().ThrowIfInvalid(model);

            user.Name = model.Name;
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.LicenceType = model.LicenceType;

            if (await _database.SaveChangesAsync(cancellationToken) != 1)
                throw new UserSaveException();
        }
    }
}
