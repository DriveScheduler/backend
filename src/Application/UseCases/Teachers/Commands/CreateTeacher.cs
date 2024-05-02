using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Validators.Users;

using MediatR;

namespace Application.UseCases.Teachers.Commands
{
    public sealed record CreateTeacher_Command(string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest<Guid>;

    internal sealed class CreateTeacher_CommandHandler(IDatabase database) : IRequestHandler<CreateTeacher_Command, Guid>
    {
        private readonly IDatabase _database = database;

        public async Task<Guid> Handle(CreateTeacher_Command request, CancellationToken cancellationToken)
        {
            Teacher user = new Teacher()
            {
                Name = request.Name,
                FirstName = request.Firstname,
                Email = request.Email,
                LicenceType = request.LicenceType,
            };

            new UserValidator().ThrowIfInvalid(user);

            _database.Teachers.Add(user);

            if (await _database.SaveChangesAsync() != 1)
            {
                throw new UserSaveException();
            }

            return user.Id;
        }
    }
}
