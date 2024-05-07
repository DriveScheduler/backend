using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Validators.Vehicles;

using MediatR;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record UpdateVehicle_Command(int Id, string RegistrationNumber, string Name, LicenceType Type) : IRequest;

    internal sealed class UpdateVehicle_CommandHandler(IDatabase database) : IRequestHandler<UpdateVehicle_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(UpdateVehicle_Command request, CancellationToken cancellationToken)
        {
            Vehicle? vehicle = _database.Vehicles.Find(request.Id);
            if (vehicle is null)
                throw new VehicleNotFoundException();

            if(vehicle.RegistrationNumber != request.RegistrationNumber)
                if (_database.Vehicles.FirstOrDefault(v => v.RegistrationNumber == request.RegistrationNumber) is not null)
                    throw new VehicleValidationException("Un véhicule avec cette immatriculation existe déjà");

            Vehicle model = new Vehicle() { Name = request.Name, RegistrationNumber = request.RegistrationNumber, Type = request.Type };
            new VehicleValidator().ThrowIfInvalid(model);
            
            vehicle.RegistrationNumber = request.RegistrationNumber;
            vehicle.Name = request.Name;
            vehicle.Type = request.Type;

            if (await _database.SaveChangesAsync() != 1)
                throw new VehicleSaveException();
        }
    }
}
