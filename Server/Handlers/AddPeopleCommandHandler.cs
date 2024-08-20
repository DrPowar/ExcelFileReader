using MediatR;
using Server.Commands;
using Server.Services;

namespace Server.Handlers
{
    public class AddPeopleCommandHandler : IRequestHandler<AddPeopleCommand, SavePeopleResult>
    {
        private readonly AddPeopleService _addPeopleService;

        public AddPeopleCommandHandler(AddPeopleService addPeopleService)
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
