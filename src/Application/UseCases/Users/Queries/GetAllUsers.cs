using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetAllUsers_Query() : IRequest<List<User>>;

    internal sealed class GetAllUsers_QueryHander(IUserRepository userRepository) : IRequestHandler<GetAllUsers_Query, List<User>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<List<User>> Handle(GetAllUsers_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRepository.GetAll().ToList());
        }
    }
}
