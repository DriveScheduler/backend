using Application.UseCases.Users.Queries;

using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Users
{
    public class UserGetAccount : IClassFixture<SetupDependencies>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UserGetAccount(SetupDependencies fixture)
        {
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
        }


        [Fact]
        public async void UserShould_GetHisAccount()
        {
            // Arrange
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const LicenceType licenceType = LicenceType.Car;

            User expectedUser = DataSet.GetStudent(userId, licenceType);
            _userRepository.Insert(expectedUser);

            // Act
            var getCommand = new GetUserById_Query(userId);
            User user = await _mediator.Send(getCommand);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(expectedUser.Name, user.Name);
            Assert.Equal(expectedUser.FirstName, user.FirstName);
            Assert.Equal(expectedUser.Email, user.Email);
            Assert.Equal(licenceType, user.LicenceType);
        }

        [Fact]
        public async void CouldNotGetInvalidUser()
        {
            // Arrange
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");

            // Act
            var getCommand = new GetUserById_Query(userId);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(getCommand));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }
    }
}
