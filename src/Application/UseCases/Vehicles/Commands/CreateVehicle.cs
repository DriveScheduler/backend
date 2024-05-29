using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;
using Domain.Exceptions.Vehicles;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record CreateVehicle_Command(string RegistrationNumber, string Name, LicenceType Type) : IRequest<int>;

    internal sealed class CreateVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<CreateVehicle_Command, int>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task<int> Handle(CreateVehicle_Command request, CancellationToken cancellationToken)
        {          
            if(_vehicleRepository.IsRegistrationNumberUnique(request.RegistrationNumber) == false) 
                throw new VehicleValidationException("Un véhicule avec cette immatriculation existe déjà");

            Vehicle vehicle = new Vehicle(request.RegistrationNumber, request.Name, request.Type);         

            _vehicleRepository.Insert(vehicle);

            return Task.FromResult(vehicle.Id);
        }
    }
}
