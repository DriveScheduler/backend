using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.Users;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserById_Query(Guid Id) : IRequest<User>;

    internal sealed class GetUserById_QueryHandler(IDatabase database) : IRequestHandler<GetUserById_Query, User>
    {
        private readonly IDatabase _database = database;

        public Task<User> Handle(GetUserById_Query request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.Id);
            if (user is null)
                throw new UserNotFoundException();

            return Task.FromResult(user);
        }
    }
}
