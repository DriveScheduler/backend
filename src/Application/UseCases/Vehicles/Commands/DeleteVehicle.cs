using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using MediatR;
using Domain.Exceptions.Vehicles;

namespace Application.UseCases.Vehicles.Commands
{
    public sealed record DeleteVehicle_Command(int Id) : IRequest;

    internal sealed class DeleteVehicle_CommandHandler(IVehicleRepository vehicleRepository) : IRequestHandler<DeleteVehicle_Command>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public Task Handle(DeleteVehicle_Command request, CancellationToken cancellationToken)
        {           
            _vehicleRepository.DeleteById(request.Id);
            return Task.CompletedTask;
        }
    }
}