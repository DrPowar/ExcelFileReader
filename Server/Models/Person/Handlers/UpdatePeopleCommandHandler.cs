using MediatR;
using Server.Models.Person.Commands;
using Server.Models.Person.Services;

namespace Server.Models.Person.Handlers
{
    public class UpdatePeopleCommandHandler : IRequestHandler<UpdatePeopleCommand, PeopleCommandResult>
    {
        private readonly IPeopleService _peopleService;

        public UpdatePeopleCommandHandler(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public async Task<PeopleCommandResult> Handle(UpdatePeopleCommand updatePeopleCommand, CancellationToken cancellationToken)
        {
            var result = await _peopleService.UpdatePeople(updatePeopleCommand.People);
            return result;
        }
    }
}
