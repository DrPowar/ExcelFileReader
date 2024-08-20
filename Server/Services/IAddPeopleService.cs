using Server.Commands;
using Server.Models;

namespace Server.Services
{
    public interface IAddPeopleService
    {
        public Task<SavePeopleResult> AddPeople(List<Person> people);
    }
}
