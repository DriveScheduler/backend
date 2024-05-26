using Domain.Entities;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetTeachers_Query() : IRequest<List<User>>;
    internal sealed class GetTeachers_QueryHandler(IUserRepository userRepository) : IRequestHandler<GetTeachers_Query, List<User>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<List<User>> Handle(GetTeachers_Query request, CancellationToken cancellationToken)
        {
            //return _database.Users
            //    .Where(u => u.Type == UserType.Teacher)
            //    .ToListAsync();
            return _userRepository.GetAllTeachers();
        }
    }
}
