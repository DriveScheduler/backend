using Domain.Enums;
using Domain.Models.Vehicles;
using Domain.Repositories;

using Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Integration
{
    public class VehicleRepository
    {
        private readonly IVehicleRepository _vehicleRepository;        

        public VehicleRepository()
        {
            SetupDependencies fixture = new SetupDependencies();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();           
        }   

        [Fact]
        public void VehicleShouldBeCreatedWithId()
        {
            Car car = new Car("XX123XX", "Voiture");

            _vehicleRepository.Insert(car);

            Assert.That(car.Id, Is.Not.EqualTo((int)default));
        }

        [Fact]
        public void VehicleShouldBeCreatedWithDifferentId()
        {
            Car car = new Car("XX123XX", "Voiture");
            Motorcycle motorcycle = new Motorcycle("YY123YY", "Moto");
            Truck truck = new Truck("2212322", "Camion");            

            _vehicleRepository.Insert([car, motorcycle, truck]);

            Assert.That(car.Id, Is.Not.EqualTo((int)default));
            Assert.That(motorcycle.Id, Is.Not.EqualTo((int)default));
            Assert.That(truck.Id, Is.Not.EqualTo((int)default));

            Assert.That(car.Id, Is.Not.EqualTo(motorcycle.Id));
            Assert.That(truck.Id, Is.Not.EqualTo(motorcycle.Id));
            Assert.That(truck.Id, Is.Not.EqualTo(car.Id));
        }

        [Fact]
        public void VehicleShouldBeGetByQuery()
        {
            Car car = new Car("XX123XX", "Voiture");

            _vehicleRepository.Insert(car);

            Vehicle vehicleFromQuery = _vehicleRepository.GetById(car.Id);
            Assert.NotNull(vehicleFromQuery);
        }

        [Fact]
        public void VehicleShouldBeUpdate()
        {
            Car car = new Car("XX123XX", "Voiture");

            _vehicleRepository.Insert(car);

            Car updatedVehicle = new Car("XX123XX", "Voiture2");
            _vehicleRepository.Update(updatedVehicle);

            Vehicle vehicleFromQuery = _vehicleRepository.GetById(car.Id);

            Assert.Equals(updatedVehicle, vehicleFromQuery);
        }
    }
}
