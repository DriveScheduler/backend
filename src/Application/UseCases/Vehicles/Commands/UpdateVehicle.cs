using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record UpdateVehicle_Command(int Id, string RegistrationNumber, string Name, LicenceType Type) : IRequest;

    internal sealed class UpdateVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<UpdateVehicle_Command>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task Handle(UpdateVehicle_Command request, CancellationToken cancellationToken)
        {
            //Vehicle? vehicle = _database.Vehicles.Find(request.Id);
            //if (vehicle is null)
            //    throw new VehicleNotFoundException();
            Vehicle vehicle = _vehicleRepository.GetById(request.Id);

            //vehicle.RegistrationNumber = request.RegistrationNumber;
            //vehicle.Name = request.Name;
            //vehicle.Type = request.Type;
            vehicle.Update(request.RegistrationNumber, request.Name);

                        
            //new VehicleValidator(_database).ThrowIfInvalid(vehicle);


            //if (await _database.SaveChangesAsync() != 1)
            //    throw new VehicleSaveException();

            _vehicleRepository.Update(vehicle);

            return Task.CompletedTask;
        }
    }
}
