using MediatR;

namespace Server.Models.Person.Commands
{
    public class DeletePeopleCommand : IRequest<PeopleCommandResult>
    {
        public List<Person> People { get; private set; }

        public DeletePeopleCommand(List<Person> people)
        {
            People = people;
        }
    }
}
