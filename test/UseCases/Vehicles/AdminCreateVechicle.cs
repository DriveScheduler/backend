﻿using Application.UseCases.Vehicles.Commands;
using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;
using Infrastructure.Persistence;
using Domain.Models.Vehicles;

namespace UseCases.Vehicles
{
    public class AdminCreateVechicle
    {
        private readonly IVehicleRepository _vehicleRepository;        
        private readonly IMediator _mediator;

        public AdminCreateVechicle()
        {
            SetupDependencies fixture = new SetupDependencies();
            fixture.BuildDefault();

            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
        }
     

        [Fact]
        public async void AdminShould_CreateAVehicle()
        {
            // Arrange
            const string registrationNumber = "AA123BB";
            const string name = "Peugeot 208";
            const LicenceType type = LicenceType.Car;

            // Act
            var command = new CreateVehicle_Command(registrationNumber, name, type);
            int vehicleId = await _mediator.Send(command);
            Vehicle? vehicle = _vehicleRepository.GetById(vehicleId);

            // Assert            
            Assert.NotNull(vehicle);
            Assert.Equal(registrationNumber, vehicle.RegistrationNumber.Value);
            Assert.Equal(name, vehicle.Name);
            Assert.IsType<Car>(vehicle);
        }

        [Theory]
        [InlineData("")]
        [InlineData("AA123B")]
        [InlineData("AA123B2")]
        [InlineData("12B23B2")]
        [InlineData("12ABC34")]
        public async void AdminShould_CreateAVehicle_WithValidRegistrationNumber(string invalidRegistrationNumber)
        {
            // Arrange            
            const string name = "Peugeot 208";
            const LicenceType type = LicenceType.Car;

            // Act
            var command = new CreateVehicle_Command(invalidRegistrationNumber, name, type);

            // Assert                        
            await Assert.ThrowsAsync<RegistrationNumberException>(() => _mediator.Send(command));          
        }

        [Fact]
        public async void AdminShould_CreateAVehicle_WithName()
        {
            // Arrange
            const string registrationNumber = "AA123BB";
            const string name = "";
            const LicenceType type = LicenceType.Car;

            // Act
            var command = new CreateVehicle_Command(registrationNumber, name, type);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom est obligatoire", exc.Message);
        }


        [Fact]
        public async void AdminShould_NotCreateAVehicle_WithRegistrationNumberThatAlreadyUsed()
        {
            // Arrange
            const string name = "Peugeot 208";
            const LicenceType type = LicenceType.Car;

            Vehicle vehicle = DataTestFactory.GetTruck(1);
            string existingRegistrationNumber = vehicle.RegistrationNumber.Value;

            _vehicleRepository.Insert(vehicle);

            // Act
            var command = new CreateVehicle_Command(existingRegistrationNumber, name, type);

            // Assert                        
            VehicleValidationException exc = await Assert.ThrowsAsync<VehicleValidationException>(() => _mediator.Send(command));
            Assert.Equal("Un véhicule avec cette immatriculation existe déjà", exc.Message);
        }
    }
}
