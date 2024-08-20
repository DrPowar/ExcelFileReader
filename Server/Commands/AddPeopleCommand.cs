using MediatR;
using Server.Models;

namespace Server.Commands
{
    public class AddPeopleCommand : IRequest<SavePeopleResult>
    {
        public List<Person> People { get; private set; }

        public AddPeopleCommand(List<Person> people)
        {
            People = people;
        }
    }
}
