using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Vehicles;

using MediatR;

namespace Application.UseCases.Vehicles.Queries
{
    public sealed record GetVehicleById_Query(int Id) : IRequest<Vehicle>;

    internal class GetVehicleById_QueryHandler(IDatabase database) : IRequestHandler<GetVehicleById_Query, Vehicle>
    {
        private readonly IDatabase _database = database;

        public Task<Vehicle> Handle(GetVehicleById_Query request, CancellationToken cancellationToken)
        {
            Vehicle? vehicle = _database.Vehicles.Find(request.Id);
            if (vehicle is null)
                throw new VehicleNotFoundException();
            return Task.FromResult(vehicle);
        }
    }
}
