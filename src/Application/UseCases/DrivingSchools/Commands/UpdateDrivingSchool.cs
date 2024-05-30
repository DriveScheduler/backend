using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.DrivingSchools.Commands
{
    public sealed record UpdateDrivingSchool_Command(int Id, string Name, string Address) : IRequest;

    internal sealed class UpdateDrivingSchool_CommandHandler(IDrivingSchoolRepository drivingSchoolRepository) : IRequestHandler<UpdateDrivingSchool_Command>
    {
        private readonly IDrivingSchoolRepository _drivingSchoolRepository = drivingSchoolRepository;

        public Task Handle(UpdateDrivingSchool_Command request, CancellationToken cancellationToken)
        {
            DrivingSchool drivingSchool = _drivingSchoolRepository.GetById(request.Id);
            drivingSchool.Update(request.Name, request.Address);
            _drivingSchoolRepository.Update(drivingSchool);
            return Task.CompletedTask;
        }
    }
}