using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Validators.Vehicles;

using MediatR;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record CreateVehicle_Command(string RegistrationNumber, string Name, LicenceType Type) : IRequest<int>;

    internal sealed class CreateVehicle_CommandHandler(IDatabase database) : IRequestHandler<CreateVehicle_Command, int>
    {
        private readonly IDatabase _database = database;

        public async Task<int> Handle(CreateVehicle_Command request, CancellationToken cancellationToken)
        {
            Vehicle? vehicle = _database.Vehicles.FirstOrDefault(v => v.RegistrationNumber == request.RegistrationNumber);
            if (vehicle is not null)
                throw new VehicleValidationException("Un véhicule avec cette immatriculation existe déjà");

            vehicle = new Vehicle()
            {                
                RegistrationNumber = request.RegistrationNumber,
                Name = request.Name,
                Type = request.Type
            };

            new VehicleValidator().ThrowIfInvalid(vehicle);

            _database.Vehicles.Add(vehicle);
            if (await _database.SaveChangesAsync() != 1)
                throw new VehicleSaveException();

            return vehicle.Id;
        }
    }
}
