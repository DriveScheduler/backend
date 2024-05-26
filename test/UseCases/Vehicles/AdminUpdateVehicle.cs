using Application.UseCases.Vehicles.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Vehicles;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Vehicles
{
    public class AdminUpdateVehicle : IClassFixture<SetupDependencies>, IDisposable
    {
        private IMediator _mediator;
        private IDatabase _database;

        public AdminUpdateVehicle(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void AdminShould_UpdateAVehicle()
        {
            // Arrange
            const int vehicleId = 1;
            const string registrationNumber = "AA123BB";
            const string updatedName = "Renault Clio";
            const LicenceType updatedType = LicenceType.Car;

            _database.Vehicles.Add(DataSet.GetTruck(vehicleId));
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateVehicle_Command(vehicleId, registrationNumber, updatedName, updatedType);
            await _mediator.Send(command);
            Vehicle? vehicle = _database.Vehicles.Find(vehicleId);

            // Assert            
            Assert.NotNull(vehicle);            
            Assert.Equal(updatedName, vehicle.Name);
            Assert.Equal(updatedType, vehicle.Type);
        }

        [Theory]
        [InlineData("")]
        [InlineData("AA123B")]
        [InlineData("AA123B2")]
        [InlineData("12B23B2")]
        [InlineData("12ABC34")]
        public async void AdminShould_UpdateAVehicle_WithValidRegistrationNumber(string invalidRegistrationNumber)
        {
            // Arrange
            const int vehicleId = 1;            

            _database.Vehicles.Add(DataSet.GetCar(vehicleId));
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateVehicle_Command(vehicleId, invalidRegistrationNumber, "Car", LicenceType.Car);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'immatriculation ne respecte pas le format XX123XX", exc.Message);
        }

        [Fact]
        public async void AdminShould_UpdateAVehicle_WithName()
        {
            // Arrange
            const int vehicleId = 1;
            const string registrationNumber = "AA123BB";
            const string name = "";
            const LicenceType type = LicenceType.Car;

            _database.Vehicles.Add(DataSet.GetCar(vehicleId));
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateVehicle_Command(vehicleId, registrationNumber, name, type);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom est obligatoire", exc.Message);
        }


        [Fact]
        public async void AdminShould_NotUpdateAVehicle_WithRegistrationNumberThatAlreadyUsed()
        {
            // Arrange
            const int vehicleId = 1;

            Vehicle car = DataSet.GetCar(2);
            string existingRegistrationNumber = car.RegistrationNumber;

            _database.Vehicles.Add(car);
            _database.Vehicles.Add(DataSet.GetMotorcycle(vehicleId));
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateVehicle_Command(vehicleId, existingRegistrationNumber, "moto", LicenceType.Motorcycle);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Un véhicule avec cette immatriculation existe déjà", exc.Message);
        }
    }
}
