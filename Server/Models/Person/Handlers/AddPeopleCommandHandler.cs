using MediatR;
using Server.Models.Person.Commands;
using Server.Models.Person.Services;

namespace Server.Models.Person.Handlers
{
    public class AddPeopleCommandHandler : IRequestHandler<AddPeopleCommand, SavePeopleResult>
    {
        private readonly IPeopleService _addPeopleService;

        public AddPeopleCommandHandler(IPeopleService addPeopleService)
        {
            _addPeopleService = addPeopleService;
        }

        public async Task<SavePeopleResult> Handle(AddPeopleCommand addPeopleCommand, CancellationToken cancellationToken)
        {
            var result = await _addPeopleService.AddPeople(addPeopleCommand.People);
            return result;
        }
    }
}
