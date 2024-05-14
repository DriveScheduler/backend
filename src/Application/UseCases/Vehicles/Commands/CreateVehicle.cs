using Domain.Abstractions;
using Domain.Entities.Database;
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
            Vehicle vehicle = new Vehicle()
            {
                RegistrationNumber = request.RegistrationNumber,
                Name = request.Name,
                Type = request.Type
            };

            new VehicleValidator(_database).ThrowIfInvalid(vehicle);

            _database.Vehicles.Add(vehicle);
            if (await _database.SaveChangesAsync() != 1)
                throw new VehicleSaveException();

            return vehicle.Id;
        }
    }
}
