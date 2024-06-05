using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetTeachers_Query() : IRequest<List<Teacher>>;
    internal sealed class GetTeachers_QueryHandler(IUserRepository userRepository) : IRequestHandler<GetTeachers_Query, List<Teacher>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<List<Teacher>> Handle(GetTeachers_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRepository.GetAllTeachers());
        }
    }
}
