using MediatR;

namespace Server.Models.Person.Commands
{
    public class UpdatePeopleCommand : IRequest<PeopleCommandResult>
    {
        public List<Person> People { get; private set; }

        public UpdatePeopleCommand(List<Person> people)
        {
            People = people;
        }
    }
}
