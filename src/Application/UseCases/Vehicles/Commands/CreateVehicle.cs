using Domain.Enums;
using Domain.Repositories;

using MediatR;
using Domain.Exceptions.Vehicles;
using Domain.Models.Vehicles;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record CreateVehicle_Command(string RegistrationNumber, string Name, LicenceType Type) : IRequest<int>;

    internal sealed class CreateVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<CreateVehicle_Command, int>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task<int> Handle(CreateVehicle_Command request, CancellationToken cancellationToken)
        {
            if (_vehicleRepository.IsRegistrationNumberUnique(request.RegistrationNumber) == false)
                throw new VehicleValidationException("Un véhicule avec cette immatriculation existe déjà");

            Vehicle vehicle = null;
            switch (request.Type)
            {
                case LicenceType.Car:
                    vehicle = new Car(request.RegistrationNumber, request.Name); break;
                case LicenceType.Truck:
                    vehicle = new Truck(request.RegistrationNumber, request.Name); break;
                case LicenceType.Motorcycle:
                    vehicle = new Motorcycle(request.RegistrationNumber, request.Name); break;
                case LicenceType.Bus:
                    vehicle = new Bus(request.RegistrationNumber, request.Name); break;
            }

            _vehicleRepository.Insert(vehicle);

            return Task.FromResult(vehicle.Id);
        }
    }
}
