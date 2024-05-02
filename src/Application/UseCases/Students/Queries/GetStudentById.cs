using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Users;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetStudentById_Query(Guid Id) : IRequest<Student>;

    internal sealed class GetStudentById_QueryHandler(IDatabase database) : IRequestHandler<GetStudentById_Query, Student>
    {
        private readonly IDatabase _database = database;

        public Task<Student> Handle(GetStudentById_Query request, CancellationToken cancellationToken)
        {
            Student? user = _database.Students.Find(request.Id);
            if (user is null)
                throw new UserNotFoundException();

            return Task.FromResult(user);
        }
    }
}
