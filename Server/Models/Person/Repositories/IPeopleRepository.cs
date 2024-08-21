using Server.Models.Person;
using Server.Models.Person.Commands;

namespace Server.Models.Person.Repositories
{
    public interface IPeopleRepository
    {
        public Task<SavePeopleResult> AddPeople(List<Person> people);
    }
}
