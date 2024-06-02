using Domain.Enums;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Integration
{
    public class VehicleRepository : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDataAccessor _database;

        public VehicleRepository(SetupDependencies fixture)
        {
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public void VehicleShouldBeCreatedWithId()
        {
            Vehicle car = new Vehicle("XX123XX", "Voiture", LicenceType.Car);

            _vehicleRepository.Insert(car);

            Assert.That(car.Id, Is.Not.EqualTo((int)default));
        }

        [Fact]
        public void VehicleShouldBeCreatedWithDifferentId()
        {
            Vehicle car = new Vehicle("XX123XX", "Voiture", LicenceType.Car);
            Vehicle car2 = new Vehicle("YY123YY", "Voiture2", LicenceType.Car);
            Vehicle car3 = new Vehicle("2212322", "Voiture3", LicenceType.Car);            

            _vehicleRepository.Insert([car, car2, car3]);

            Assert.That(car.Id, Is.Not.EqualTo((int)default));
            Assert.That(car2.Id, Is.Not.EqualTo((int)default));
            Assert.That(car3.Id, Is.Not.EqualTo((int)default));

            Assert.That(car.Id, Is.Not.EqualTo(car2.Id));
            Assert.That(car3.Id, Is.Not.EqualTo(car2.Id));
            Assert.That(car3.Id, Is.Not.EqualTo(car.Id));
        }

        [Fact]
        public void VehicleShouldBeGetByQuery()
        {
            Vehicle car = new Vehicle("XX123XX", "Voiture", LicenceType.Car);

            _vehicleRepository.Insert(car);

            Vehicle vehicleFromQuery = _vehicleRepository.GetById(car.Id);
            Assert.NotNull(vehicleFromQuery);
        }

        [Fact]
        public void VehicleShouldBeUpdate()
        {
            Vehicle car = new Vehicle("XX123XX", "Voiture", LicenceType.Car);

            _vehicleRepository.Insert(car);

            Vehicle updatedVehicle = new Vehicle("XX123XX", "Voiture2", LicenceType.Car);
            _vehicleRepository.Update(updatedVehicle);

            Vehicle vehicleFromQuery = _vehicleRepository.GetById(car.Id);

            Assert.Equals(updatedVehicle, vehicleFromQuery);
        }
    }
}
