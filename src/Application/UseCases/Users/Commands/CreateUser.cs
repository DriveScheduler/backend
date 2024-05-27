using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record CreateUser_Command(string Name, string Firstname, string Email, string Password, LicenceType LicenceType, UserType Type) : IRequest<Guid>;

    internal sealed class CreateUser_CommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUser_Command, Guid>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<Guid> Handle(CreateUser_Command request, CancellationToken cancellationToken)
        {
            User user = new User(
                request.Name,
                request.Firstname,
                request.Email,
                request.Password,
                request.LicenceType,
                request.Type);
            //User user = new User()
            //{
            //    Name = request.Name,
            //    FirstName = request.Firstname,
            //    Email = request.Email,
            //    LicenceType = request.LicenceType,
            //    Type = request.Type,
            //    Password = request.Password
            //};

            //new UserValidator(_database).ThrowIfInvalid(user);

            return _userRepository.InsertAsync(user);
            //_database.Users.Add(user);

            //if (await _database.SaveChangesAsync() != 1)
            //{
            //    throw new UserSaveException();
            //}

            //return user.Id;
        }
    }
}
