using MediatR;
using Server.Models.Person.Commands;
using Server.Models.Person.Services;

namespace Server.Models.Person.Handlers
{
    public class DeletePeopleCommandHandler : IRequestHandler<DeletePeopleCommand, PeopleCommandResult>
    {
        private readonly IPeopleService _peopleService;

        public DeletePeopleCommandHandler(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public async Task<PeopleCommandResult> Handle(DeletePeopleCommand deletePeopleCommand, CancellationToken cancellationToken)
        {
            var result = await _peopleService.DeletePeople(deletePeopleCommand.People);
            return result;
        }
    }
}
