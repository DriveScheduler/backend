using Application.UseCases.Users.Commands;

using Domain.Abstractions;
using Domain.Enums;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace UseCases.Users
{
    public class UserCreateAccount : IClassFixture<SetupDependencies>, IDisposable
    {
        private IMediator _mediator;
        private IDatabase _database;

        public UserCreateAccount(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void UserShould_CreateAnAccount()
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;            

            // Act
            var command = new CreateStudent_Command(name, firstname, email, licenceType);
            Guid userId = await _mediator.Send(command);

            // Assert
            Assert.NotEqual(Guid.Empty, userId);
            Assert.NotNull(_database.Students.Find(userId));
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithName()
        {
            // Arrange
            const string name = "";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;            

            // Act
            var command = new CreateStudent_Command(name, firstname, email, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithFirstName()
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;            

            // Act
            var command = new CreateStudent_Command(name, firstname, email, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le prénom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithEmail()
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "John";
            const string email = "";
            const LicenceType licenceType = LicenceType.Car;            

            // Act
            var command = new CreateStudent_Command(name, firstname, email, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'adresse email est obligatoire", exc.Message);
        }

        [Theory]
        [InlineData("jonhdoegmailcom")]
        [InlineData("jonh.doegmail.com")]
        [InlineData("@gmail.com")]
        [InlineData("@g.com")]
        public async void UserShould_CreateAnAccount_WithValidEmail(string invalidEmail)
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "John";
            const LicenceType licenceType = LicenceType.Car;            

            // Act
            var command = new CreateStudent_Command(name, firstname, invalidEmail, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'adresse email n'est pas valide", exc.Message);
        }
    }
}