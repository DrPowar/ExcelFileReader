using Server.Models.Person;
using Server.Models.Person.Commands;

namespace Server.Models.Person.Services
{
    public interface IAddPeopleService
    {
        public Task<SavePeopleResult> AddPeople(List<Person> people);
    }
}
