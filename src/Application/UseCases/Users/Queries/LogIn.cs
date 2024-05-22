using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.Users;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record LogIn_Query(string Email, string Password) : IRequest<User>;

    internal sealed class ogIn_QueryHandler(IDatabase database) : IRequestHandler<LogIn_Query, User>
    {
        private readonly IDatabase _database = database;

        public Task<User> Handle(LogIn_Query request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user is null)
                throw new UserNotFoundException();

            if (user.Password != request.Password)
                throw new UserValidationException("Mot de passe incorrect");

            return Task.FromResult(user);
        }
    }
}
