using MediatR;

namespace Server.Models.Person.Commands
{
    public class UpdatePeopleCommand : IRequest<PeopleCommandResult>
    {
        public List<UpdatedPerson> People { get; private set; }

        public UpdatePeopleCommand(List<UpdatedPerson> people)
        {
            People = people;
        }
    }
}
