using Microsoft.AspNetCore.Identity;
using Server.Commands;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    internal class AddPersonService : IAddPersonService
    {
        private readonly IPersonRepository _personRepository;

        internal AddPersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public async Task<SavePersonResult> AddPerson(Person person)
        {
            return await _personRepository.AddPerson(person);
        }
    }
}