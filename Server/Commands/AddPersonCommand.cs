using MediatR;
using Server.Models;

namespace Server.Commands
{
    public class AddPersonCommand : IRequest<SavePersonResult>
    {
        public Person Person { get; private set; }

        public AddPersonCommand(Person person)
        {
            Person = person;
        }
    }
}
