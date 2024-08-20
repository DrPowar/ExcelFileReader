using Server.Commands;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class AddPeopleService : IAddPeopleService
    {
        private readonly IPersonRepository _personRepository;

        public AddPeopleService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<SavePeopleResult> AddPeople(List<Person> people)
        {
            return await _personRepository.AddPeople(people);
        }
    }
}
