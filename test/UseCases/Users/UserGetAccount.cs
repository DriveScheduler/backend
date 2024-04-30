using Application.UseCases.Users.Queries;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace UseCases.Users
{
    public class UserGetAccount : IClassFixture<SetupDependencies>, IDisposable
    {
        private IMediator _mediator;
        private IDatabase _database;

        public UserGetAccount(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void UserShould_GetHisAccount()
        {
            // Arrange
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;
            const UserType userType = UserType.Student;

            _database.Users.Add(new User() { Id = userId, Name = name, Firstname = firstname, Email = email, LicenceType = licenceType, UserType = userType });
            await _database.SaveChangesAsync();

            // Act
            var getCommand = new GetUserById_Query(userId);
            User user = await _mediator.Send(getCommand);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(name, user.Name);
            Assert.Equal(firstname, user.Firstname);
            Assert.Equal(email, user.Email);
            Assert.Equal(licenceType, user.LicenceType);
            Assert.Equal(userType, user.UserType);
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
