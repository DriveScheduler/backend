using Domain.Abstractions;
using Domain.Enums;

using MediatR;

namespace Application.UseCases.Teachers.Commands
{
    public sealed record UpdateTeacher_Command(Guid TeacherId, string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest;

    internal sealed class UpdateTeacher(IDatabase database) : IRequestHandler<UpdateTeacher_Command>
    {
        private readonly IDatabase _database = database;

        public Task Handle(UpdateTeacher_Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
