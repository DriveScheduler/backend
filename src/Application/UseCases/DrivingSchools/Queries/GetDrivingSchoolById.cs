using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.DrivingSchools;

using MediatR;

namespace Application.UseCases.DrivingSchools.Queries
{
    public sealed record GetDrivingSchoolById_Query(int Id) : IRequest<DrivingSchool>;

    internal class GetDrivingSchoolById_QueryHandler(IDatabase database) : IRequestHandler<GetDrivingSchoolById_Query, DrivingSchool>
    {
        private readonly IDatabase _database = database;

        public Task<DrivingSchool> Handle(GetDrivingSchoolById_Query request, CancellationToken cancellationToken)
        {
            DrivingSchool? drivingSchool = _database.DrivingSchools.Find(request.Id);
            if (drivingSchool is null)
                throw new DrivingSchoolNotFoundException();
            return Task.FromResult(drivingSchool);
        }
    }
}