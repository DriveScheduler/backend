using Domain.Entities;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.DrivingSchools.Queries
{
    public sealed record GetAllDrivingSchool_Query() : IRequest<List<DrivingSchool>>;

    internal class GetAllDrivingSchool_QueryHandler(IDrivingSchoolRepository drivingSchoolRepository) : IRequestHandler<GetAllDrivingSchool_Query, List<DrivingSchool>>
    {
        private readonly IDrivingSchoolRepository _drivingSchoolRepository = drivingSchoolRepository;

        public Task<List<DrivingSchool>> Handle(GetAllDrivingSchool_Query request, CancellationToken cancellationToken)
        {
            return _drivingSchoolRepository.GetAllAsync();
        }
    }
}