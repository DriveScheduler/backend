using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record CreateStudent_Command(string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest<Guid>;

    internal sealed class CreateStudent_RequestHandler(IDatabase database) : IRequestHandler<CreateStudent_Command, Guid>
    {
        private readonly IDatabase _database = database;

        public async Task<Guid> Handle(CreateStudent_Command request, CancellationToken cancellationToken)
        {
            Student user = new Student()
            {
                Name = request.Name,
                FirstName = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType,
            };

            new UserValidator().ThrowIfInvalid(user);
           
            _database.Students.Add(user);

            if (await _database.SaveChangesAsync() != 1)
            {
                throw new UserSaveException();
            }

            return user.Id;
        }
    }
}
