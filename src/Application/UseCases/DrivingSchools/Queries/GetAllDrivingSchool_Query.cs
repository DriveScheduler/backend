using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.DrivingSchools;

using MediatR;

namespace Application.UseCases.DrivingSchools.Queries
{
    public sealed record GetAllDrivingSchool_Query() : IRequest<List<DrivingSchool>>;

    internal class GetAllDrivingSchool_QueryHandler(IDatabase database) : IRequestHandler<GetAllDrivingSchool_Query, List<DrivingSchool>>
    {
        private readonly IDatabase _database = database;

        public Task<List<DrivingSchool>> Handle(GetAllDrivingSchool_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_database.DrivingSchools.ToList());
        }
    }
}