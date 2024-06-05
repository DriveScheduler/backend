using Domain.Models.Vehicles;
using Domain.Repositories;

using Microsoft.Extensions.DependencyInjection;

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

            Assert.NotEqual((int)default, car.Id);
        }

        [Fact]
        public void VehicleShouldBeCreatedWithDifferentId()
        {
            Car car = new Car("XX123XX", "Voiture");
            Motorcycle motorcycle = new Motorcycle("YY123YY", "Moto");
            Truck truck = new Truck("ZZ123ZZ", "Camion");            

            _vehicleRepository.Insert([car, motorcycle, truck]);

            Assert.NotEqual(default, car.Id);
            Assert.NotEqual(default, motorcycle.Id);
            Assert.NotEqual(default, truck.Id);

            Assert.NotEqual(motorcycle.Id, car.Id);
            Assert.NotEqual(motorcycle.Id, truck.Id);
            Assert.NotEqual(car.Id, truck.Id);
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

            Vehicle updatedVehicle = _vehicleRepository.GetById(car.Id);
            updatedVehicle.Update("YY456YY", "Voiture 2");
            _vehicleRepository.Update(updatedVehicle);           

            Vehicle vehicleFromQuery = _vehicleRepository.GetById(car.Id);

            Assert.Equal(updatedVehicle, vehicleFromQuery);
        }
    }
}
