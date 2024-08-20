using MediatR;
using Server.Commands;
using Server.Services;

namespace Server.Handlers
{
    internal sealed class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, SavePersonResult>
    {
        private readonly AddPersonService _addUserService;

        internal AddPersonCommandHandler(AddPersonService addUserService)
        {
            _addUserService = addUserService;
        }

        public async Task<SavePersonResult> Handle(AddPersonCommand addPersonCommand, CancellationToken cancellationToken)
        {
            var result = await _addUserService.AddPerson(addPersonCommand.Person);
            return result;
        }
    }
}
