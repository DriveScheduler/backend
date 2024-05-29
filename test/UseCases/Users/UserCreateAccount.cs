using Application.UseCases.Users.Commands;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;
using Infrastructure.Persistence;

namespace UseCases.Users
{
    public class UserCreateAccount : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;

        public UserCreateAccount(SetupDependencies fixture)
        {
            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
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
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, email, password, licenceType, UserType.Student);
            Guid userId = await _mediator.Send(command);

            // Assert
            Assert.NotEqual(Guid.Empty, userId);
            Assert.NotNull(_userRepository.GetUserById(userId));
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithName()
        {
            // Arrange
            const string name = "";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, email, password, licenceType, UserType.Student);

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
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, email, password, licenceType, UserType.Student);

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
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, email, password, licenceType, UserType.Student);

            // Assert
            await Assert.ThrowsAsync<InvalidEmailException>(() => _mediator.Send(command));            
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
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, invalidEmail, password, licenceType, UserType.Student);

            // Assert
            await Assert.ThrowsAsync<InvalidEmailException>(() => _mediator.Send(command));            
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithUnusedEmail()
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "John";
            const string password = "mdp123";
            const LicenceType licenceType = LicenceType.Car;

            User user = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000001"));
            string existingEmail = user.Email.Value;
            _userRepository.Insert(user);

            // Act
            var command = new CreateUser_Command(name, firstname, existingEmail, password, licenceType, UserType.Student);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'adresse email est déjà utilisée", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithPassword()
        {
            // Arrange
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const string password = "";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new CreateUser_Command(name, firstname, email, password, licenceType, UserType.Student);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le mot de passe est obligatoire", exc.Message);
        }
    }
}