using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record UpdateStudent_Command(Guid UserId, string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest;

    internal sealed class UpdateUser_RequestHandler(IDatabase database) : IRequestHandler<UpdateStudent_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(UpdateStudent_Command request, CancellationToken cancellationToken)
        {
            Student? user = _database.Students.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Student model = new Student()
            {
                Id = request.UserId,
                Name = request.Name,
                FirstName = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType
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
