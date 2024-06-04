using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Vehicles.Queries
{
    public sealed record GetAllVehicles_Query() : IRequest<List<Vehicle>>;

    internal sealed class GetAllVehicles_QueryHandler(IVehicleRepository vehicleRepository): IRequestHandler<GetAllVehicles_Query, List<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task<List<Vehicle>> Handle(GetAllVehicles_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_vehicleRepository.GetAll().ToList());
        }
    }
}
