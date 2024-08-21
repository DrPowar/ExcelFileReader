using MediatR;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;
using Server.Models.Person.Services;

namespace Server.Models.Person.Handlers
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, GetPeopleResult>
    {
        private readonly IPeopleService _getAllPeopleService;

        public GetAllPeopleQueryHandler(IPeopleService getAllPeopleService)
        {
            _getAllPeopleService = getAllPeopleService;
        }
        public async Task<GetPeopleResult> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            GetPeopleResult people = await _getAllPeopleService.GetAllPeople();

            return people;        
        }
    }
}
