using Domain.Entities;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Vehicles.Queries
{
    public sealed record GetVehicleById_Query(int Id) : IRequest<Vehicle>;

    internal class GetVehicleById_QueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetVehicleById_Query, Vehicle>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task<Vehicle> Handle(GetVehicleById_Query request, CancellationToken cancellationToken)
        {
            //Vehicle? vehicle = _database.Vehicles.Find(request.Id);
            //if (vehicle is null)
            //    throw new VehicleNotFoundException();
            //return Task.FromResult(vehicle);
            return _vehicleRepository.GetVehicleByIdAsync(request.Id);
        }
    }
}
