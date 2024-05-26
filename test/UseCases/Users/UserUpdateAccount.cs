using Application.UseCases.Users.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;

using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Users
{
    public class UserUpdateAccount : IClassFixture<SetupDependencies>, IDisposable
    {
        private IMediator _mediator;
        private IDatabase _database;

        public UserUpdateAccount(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void UserShould_UpdateHisAccount()
        {
            // Arrange
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string updatedName = "Doe";
            const string updatedFirstname = "John";
            const string updatedEmail = "john.doe@gmail.com";
            const LicenceType updatedLicenceType = LicenceType.Motorcycle;

            _database.Users.Add(DataSet.GetCarStudent(userId));
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, updatedName, updatedFirstname, updatedEmail, updatedLicenceType);
            await _mediator.Send(updateCommand);
            User? user = _database.Users.Find(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            Assert.Equal(updatedName, user.Name);
            Assert.Equal(updatedFirstname, user.FirstName);
            Assert.Equal(updatedEmail, user.Email);
            Assert.Equal(updatedLicenceType, user.LicenceType);
        }

        [Fact]
        public async void UserShould_UpdateHisAccountWithName()
        {
            // Arrange        
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            _database.Users.Add(DataSet.GetStudent(userId, licenceType));
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, string.Empty, firstname, email, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("Le nom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccountWithFirstName()
        {
            // Arrange       
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            _database.Users.Add(DataSet.GetStudent(userId, licenceType));
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, string.Empty, email, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("Le prénom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccountWithEmail()
        {
            // Arrange       
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            _database.Users.Add(DataSet.GetStudent(userId, licenceType));
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, firstname, string.Empty, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
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
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            _database.Users.Add(DataSet.GetStudent(userId, licenceType));
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, firstname, invalidEmail, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("L'adresse email n'est pas valide", exc.Message);
        }

        [Fact]
        public async void UserShould_CreateAnAccount_WithUnusedEmail()
        {
            // Arrange        
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            User user = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000002"));
            string existingEmail = user.Email;
            _database.Users.Add(DataSet.GetStudent(userId, licenceType));
            _database.Users.Add(user);
            await _database.SaveChangesAsync();

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, firstname, existingEmail, licenceType);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("L'adresse email est déjà utilisée", exc.Message);
        }

        [Fact]
        public async void CanNotUpdateInvalidUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            // Act
            var command = new UpdateUser_Command(userId, name, firstname, email, licenceType);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }
    }
}
