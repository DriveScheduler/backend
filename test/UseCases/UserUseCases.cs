using Application;
using Application.Abstractions;
using Application.UseCases.Users.Commands;

using Domain.Enums;
using Domain.Exceptions.Users;

using Infrastructure;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace UseCases
{
    public class SetupMediator
    {
        public IServiceProvider ServiceProvider { get; }

        public SetupMediator()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.ApplicationDependencyInjection();
            serviceCollection.SetupDatabaseInMemory();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }


    public class UserUseCases : IClassFixture<SetupMediator>
    {
        private IMediator _mediator;
        private IDatabase _database;

        public UserUseCases(SetupMediator fixture)
        {
            _mediator = fixture.ServiceProvider.GetService<IMediator>()!;
            _database = fixture.ServiceProvider.GetService<IDatabase>()!;
        }


        [Fact]
        public async void UserShouldCreateAnAccount()
        {
            const string name = "Doe";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            var command = new CreateUser_Command(name, firstname, email, licenceType);

            Guid userId = await _mediator.Send(command);

            Assert.NotEqual(Guid.Empty, userId);
            Assert.Equal(1, _database.Users.Count());
        }

        [Fact]
        public async void UserShouldCreateAnAccountWithName()
        {
            const string name = "";
            const string firstname = "John";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            var command = new CreateUser_Command(name, firstname, email, licenceType);

            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));

            Assert.Equal("Le nom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShouldCreateAnAccountWithFirstName()
        {
            const string name = "Doe";
            const string firstname = "";
            const string email = "john.doe@gmail.com";
            const LicenceType licenceType = LicenceType.Car;

            var command = new CreateUser_Command(name, firstname, email, licenceType);

            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));

            Assert.Equal("Le prénom est obligatoire", exc.Message);
        }

        [Fact]
        public async void UserShouldCreateAnAccountWithEmail()
        {
            const string name = "Doe";
            const string firstname = "John";
            const string email = "";
            const LicenceType licenceType = LicenceType.Car;

            var command = new CreateUser_Command(name, firstname, email, licenceType);

            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));

            Assert.Equal("L'adresse email est obligatoire", exc.Message);
        }

        [Theory]
        [InlineData("jonhdoegmailcom")]
        [InlineData("jonh.doegmail.com")]
        [InlineData("@gmail.com")]
        [InlineData("@g.com")]
        public async void UserShouldCreateAnAccountWithValidEmail(string invalidEmail)
        {
            const string name = "Doe";
            const string firstname = "John";
            const LicenceType licenceType = LicenceType.Car;

            var command = new CreateUser_Command(name, firstname, invalidEmail, licenceType);

            UserValidationException exc = await Assert.ThrowsAsync<UserValidationException>(() => _mediator.Send(command));

            Assert.Equal("L'adresse email n'est pas valide", exc.Message);
        }
    }
}