using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;
using Domain.Exceptions.Vehicles;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record UpdateVehicle_Command(int Id, string RegistrationNumber, string Name) : IRequest;

    internal sealed class UpdateVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<UpdateVehicle_Command>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task Handle(UpdateVehicle_Command request, CancellationToken cancellationToken)
        {           
            Vehicle vehicle = _vehicleRepository.GetById(request.Id);
            if (vehicle.RegistrationNumber.Value != request.RegistrationNumber && _vehicleRepository.IsRegistrationNumberUnique(request.RegistrationNumber) == false)
                throw new VehicleValidationException("Un véhicule avec cette immatriculation existe déjà");
         
            vehicle.Update(request.RegistrationNumber, request.Name);
                        
            _vehicleRepository.Update(vehicle);

            return Task.CompletedTask;
        }
    }
}
