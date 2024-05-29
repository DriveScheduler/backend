using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.DrivingSchools.Commands
{
    public sealed record CreateDrivingSchool_Command(string Name, string Address) : IRequest<int>;

    internal sealed class CreateDrivingSchool_CommandHandler(IDrivingSchoolRepository drivingSchoolRepository) : IRequestHandler<CreateDrivingSchool_Command, int>
    {
        private readonly IDrivingSchoolRepository _drivingSchoolRepository = drivingSchoolRepository;

        public Task<int> Handle(CreateDrivingSchool_Command request, CancellationToken cancellationToken)
        {         
            DrivingSchool drivingSchool = new DrivingSchool(request.Name, request.Address);
            _drivingSchoolRepository.Insert(drivingSchool);
            return Task.FromResult(drivingSchool.Id);
        }
    }
}