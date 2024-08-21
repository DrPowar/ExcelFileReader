using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;

namespace Server.Models.Person.Services
{
    public interface IPeopleService
    {
        public Task<SavePeopleResult> AddPeople(List<Person> people);

        public Task<GetPeopleResult> GetAllPeople();
    }
}
