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
    public class UserUpdateAccount : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;

        public UserUpdateAccount(SetupDependencies fixture)
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
        public async void UserShould_UpdateHisAccount()
        {
            // Arrange
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string updatedName = "Doe";
            const string updatedFirstname = "John";
            const string updatedEmail = "john.doe@gmail.com";            

            _userRepository.Insert(DataSet.GetCarStudent(userId));

            // Act
            var updateCommand = new UpdateUser_Command(userId, updatedName, updatedFirstname, updatedEmail);
            await _mediator.Send(updateCommand);
            User? user = _userRepository.GetUserById(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            Assert.Equal(updatedName, user.Name.Value);
            Assert.Equal(updatedFirstname, user.FirstName.Value);
            Assert.Equal(updatedEmail, user.Email.Value);            
        }

        [Fact]
        public async void UserShould_UpdateHisAccountWithName()
        {
            // Arrange        
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";            

            _userRepository.Insert(DataSet.GetCarStudent(userId));

            // Act
            var updateCommand = new UpdateUser_Command(userId, string.Empty, firstname, email);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("Le nom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_UpdateAnAccountWithFirstName()
        {
            // Arrange       
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";            

            _userRepository.Insert(DataSet.GetCarStudent(userId));

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, string.Empty, email);

            // Assert
            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(updateCommand));
            Assert.Equal("Le prénom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShould_UpdateAnAccountWithEmail()
        {
            // Arrange       
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";            

            _userRepository.Insert(DataSet.GetCarStudent(userId));

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, firstname, string.Empty);

            // Assert
            await Assert.ThrowsAsync<InvalidEmailException>(() => _mediator.Send(updateCommand));            
        }

        [Theory]
        [InlineData("jonhdoegmailcom")]
        [InlineData("jonh.doegmail.com")]
        [InlineData("@gmail.com")]
        [InlineData("@g.com")]
        public async void UserShould_UpdateAnAccount_WithValidEmail(string invalidEmail)
        {
            // Arrange        
            Guid userId = new Guid("00000000-0000-0000-0000-000000000001");
            const string name = "Doe";
            const string firstname = "John";                        

            _userRepository.Insert(DataSet.GetCarStudent(userId));

            // Act
            var updateCommand = new UpdateUser_Command(userId, name, firstname, invalidEmail);

            // Assert
            await Assert.ThrowsAsync<InvalidEmailException>(() => _mediator.Send(updateCommand));            
        }

        [Fact]
        public async void UserShould_UpdateAnAccount_WithUnusedEmail()
        {
            // Arrange                    
            const string name = "Doe";
            const string firstname = "John";            

            User user1 = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000001"));
            User user2 = DataSet.GetStudent(id:new Guid("00000000-0000-0000-0000-000000000002"), email:"test.test@gmail.com");
            string existingEmail = user1.Email.Value;            
            _userRepository.Insert(user1);
            _userRepository.Insert(user2);

            // Act
            var updateCommand = new UpdateUser_Command(user2.Id, name, firstname, existingEmail);

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

            // Act
            var command = new UpdateUser_Command(userId, name, firstname, email);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }
    }
}
