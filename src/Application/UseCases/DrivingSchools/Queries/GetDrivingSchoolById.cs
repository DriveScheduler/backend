using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.DrivingSchools.Queries
{
    public sealed record GetDrivingSchoolById_Query(int Id) : IRequest<DrivingSchool>;

    internal class GetDrivingSchoolById_QueryHandler(IDrivingSchoolRepository drivingSchoolRepository) : IRequestHandler<GetDrivingSchoolById_Query, DrivingSchool>
    {
        private readonly IDrivingSchoolRepository _drivingSchoolRepository = drivingSchoolRepository;

        public Task<DrivingSchool> Handle(GetDrivingSchoolById_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_drivingSchoolRepository.GetById(request.Id));
        }
    }
}