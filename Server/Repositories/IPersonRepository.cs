using Server.Commands;
using Server.Models;

namespace Server.Repositories
{
    public interface IPersonRepository
    {
        public Task<SavePersonResult> AddPerson(Person person);
        public Task<SavePeopleResult> AddPeople(List<Person> people); 
    }
}
