using Application.UseCases.Vehicles.Commands;

using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Vehicles
{
    public class AdminUpdateVehicle : IClassFixture<SetupDependencies>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMediator _mediator;

        public AdminUpdateVehicle(SetupDependencies fixture)
        {
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async void AdminShould_UpdateAVehicle()
        {
            // Arrange
            const int vehicleId = 1;
            const string registrationNumber = "AA123BB";
            const string updatedName = "Renault Clio";
            const LicenceType updatedType = LicenceType.Car;

            _vehicleRepository.Insert(DataSet.GetTruck(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, registrationNumber, updatedName, updatedType);
            await _mediator.Send(command);
            Vehicle? vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);

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

            _vehicleRepository.Insert(DataSet.GetCar(vehicleId));

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

            _vehicleRepository.Insert(DataSet.GetCar(vehicleId));

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
            string existingRegistrationNumber = car.RegistrationNumber.Value;

            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(DataSet.GetMotorcycle(vehicleId));

            // Act
            var command = new UpdateVehicle_Command(vehicleId, existingRegistrationNumber, "moto", LicenceType.Motorcycle);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Un véhicule avec cette immatriculation existe déjà", exc.Message);
        }
    }
}
