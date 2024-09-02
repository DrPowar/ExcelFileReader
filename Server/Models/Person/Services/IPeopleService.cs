using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;

namespace Server.Models.Person.Services
{
    public interface IPeopleService
    {
        public Task<PeopleCommandResult> AddPeople(List<Person> people);

        public Task<PeopleCommandResult> DeletePeople(List<Person> people);

        public Task<PeopleCommandResult> UpdatePeople(List<UpdatedPerson> people);

        public Task<GetPeopleResult> GetAllPeople();
    }
}
