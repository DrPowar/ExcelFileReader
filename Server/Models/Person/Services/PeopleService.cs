using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;
using Server.Models.Person.Repositories;

namespace Server.Models.Person.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _personRepository;

        public PeopleService(IPeopleRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PeopleCommandResult> AddPeople(List<Person> people)
        {
            return await _personRepository.AddPeople(people);
        }

        public async Task<PeopleCommandResult> DeletePeople(List<Person> people)
        {
            return await _personRepository.DeletePeople(people);
        }

        public async Task<GetPeopleResult> GetAllPeople()
        {
            return await _personRepository.GetAllPeople();
        }

        public async Task<PeopleCommandResult> UpdatePeople(List<Person> people)
        {
            return await _personRepository.UpdatePeople(people);
        }
    }
}
