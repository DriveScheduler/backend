using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetTeachers_Query() : IRequest<List<User>>;
    internal sealed class GetTeachers_QueryHandler(IDatabase database) : IRequestHandler<GetTeachers_Query, List<User>>
    {
        private readonly IDatabase _database = database;

        public Task<List<User>> Handle(GetTeachers_Query request, CancellationToken cancellationToken)
        {
            return _database.Users
                .Where(u => u.Type == UserType.Teacher)
                .ToListAsync();
        }
    }
}
