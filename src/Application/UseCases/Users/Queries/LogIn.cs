﻿using Domain.Exceptions.Users;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record LogIn_Query(string Email, string Password) : IRequest<User>;

    internal sealed class LogIn_QueryHandler(IUserRepository userRepository) : IRequestHandler<LogIn_Query, User>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<User> Handle(LogIn_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserByEmail(request.Email);

            if (user.Password.Value != request.Password)
                throw new UserValidationException("Mot de passe incorrect");

            return Task.FromResult(user);
        }
    }
}
