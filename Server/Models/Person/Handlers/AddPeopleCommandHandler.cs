using MediatR;
using Server.Models.Person.Commands;
using Server.Models.Person.Services;

namespace Server.Models.Person.Handlers
{
    public class AddPeopleCommandHandler : IRequestHandler<AddPeopleCommand, PeopleCommandResult>
    {
        private readonly IPeopleService _peopleService;

        public AddPeopleCommandHandler(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public async Task<PeopleCommandResult> Handle(AddPeopleCommand addPeopleCommand, CancellationToken cancellationToken)
        {
            var result = await _peopleService.AddPeople(addPeopleCommand.People);
            return result;
        }
    }
}
