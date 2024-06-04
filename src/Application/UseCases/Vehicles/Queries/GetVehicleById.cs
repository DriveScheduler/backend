using Domain.Models.Vehicles;
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
            return Task.FromResult(_vehicleRepository.GetById(request.Id));
        }
    }
}
