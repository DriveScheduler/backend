using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record CreateVehicle_Command(string RegistrationNumber, string Name, LicenceType Type) : IRequest<int>;

    internal sealed class CreateVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<CreateVehicle_Command, int>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public async Task<int> Handle(CreateVehicle_Command request, CancellationToken cancellationToken)
        {
            //Vehicle vehicle = new Vehicle()
            //{
            //    RegistrationNumber = request.RegistrationNumber,
            //    Name = request.Name,
            //    Type = request.Type
            //};
            Vehicle vehicle = new Vehicle(request.RegistrationNumber, request.Name, request.Type);

            //new VehicleValidator(_database).ThrowIfInvalid(vehicle);

            //_database.Vehicles.Add(vehicle);
            //if (await _database.SaveChangesAsync() != 1)
            //    throw new VehicleSaveException();            

            return await _vehicleRepository.InsertAsync(vehicle);
        }
    }
}
