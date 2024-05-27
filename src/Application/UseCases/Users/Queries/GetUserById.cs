﻿using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserById_Query(Guid Id) : IRequest<User>;

    internal sealed class GetUserById_QueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserById_Query, User>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<User> Handle(GetUserById_Query request, CancellationToken cancellationToken)
        {
            //User? user = _database.Users.Find(request.Id);
            //if (user is null)
            //    throw new UserNotFoundException();

            //return Task.FromResult(user);
            return _userRepository.GetUserByIdAsync(request.Id);
        }
    }
}
