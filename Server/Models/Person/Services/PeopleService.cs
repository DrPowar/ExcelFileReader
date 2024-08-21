using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Repositories;

namespace Server.Models.Person.Services
{
    public class PeopleService : IAddPeopleService
    {
        private readonly IPeopleRepository _personRepository;

        public PeopleService(IPeopleRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<SavePeopleResult> AddPeople(List<Person> people)
        {
            return await _personRepository.AddPeople(people);
        }
    }
}
