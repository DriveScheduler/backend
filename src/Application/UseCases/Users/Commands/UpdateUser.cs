using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Commands
{
    public sealed record UpdateUser_Command(Guid UserId, string Name, string Firstname, string Email, LicenceType LicenceType) : IRequest;

    internal sealed class UpdateUser_CommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUser_Command>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Handle(UpdateUser_Command request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetUserByIdAsync(request.UserId);
            //User? user = _database.Users.Find(request.UserId);
            //if (user is null)
            //    throw new UserNotFoundException();          

            user.Update(request.Name, request.Firstname, request.Email);
            //user.Name = request.Name;
            //user.FirstName = request.Firstname;
            //user.Email = request.Email;
            //user.LicenceType = request.LicenceType;

            //new UserValidator(_database).ThrowIfInvalid(user);

            await _userRepository.UpdateAsync(user);
            //if (await _database.SaveChangesAsync(cancellationToken) != 1)
            //    throw new UserSaveException();
        }
    }
}
