using Domain.Entities;
using Domain.Exceptions.DrivingSchools;
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
            //DrivingSchool? drivingSchool = _database.DrivingSchools.Find(request.Id);
            //if (drivingSchool is null)
            //    throw new DrivingSchoolNotFoundException();
            //return Task.FromResult(drivingSchool);
            return _drivingSchoolRepository.GetByIdAsync(request.Id);
        }
    }
}