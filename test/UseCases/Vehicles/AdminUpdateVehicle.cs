using Application.UseCases.Vehicles.Commands;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;
using Infrastructure.Persistence;

namespace UseCases.Vehicles
{
    public class AdminUpdateVehicle : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;

        public AdminUpdateVehicle(SetupDependencies fixture)
        {
            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
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

            _vehicleRepository.Insert(DataSet.GetTruck(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, registrationNumber, updatedName);
            await _mediator.Send(command);
            Vehicle? vehicle = _vehicleRepository.GetById(vehicleId);

            // Assert            
            Assert.NotNull(vehicle);
            Assert.Equal(updatedName, vehicle.Name);            
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

            _vehicleRepository.Insert(DataSet.GetCar(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, invalidRegistrationNumber, "Car");

            // Assert                        
            await Assert.ThrowsAsync<RegistrationNumberException>(() => _mediator.Send(command));           
        }

        [Fact]
        public async void AdminShould_UpdateAVehicle_WithName()
        {
            // Arrange
            const int vehicleId = 1;
            const string registrationNumber = "AA123BB";
            const string name = "";            

            _vehicleRepository.Insert(DataSet.GetCar(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, registrationNumber, name);

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
            string existingRegistrationNumber = car.RegistrationNumber.Value;

            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(DataSet.GetMotorcycle(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, existingRegistrationNumber, "moto");

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Un véhicule avec cette immatriculation existe déjà", exc.Message);
        }
    }
}
