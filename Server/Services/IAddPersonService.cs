using Server.Commands;
using Server.Models;

namespace Server.Services
{
    public interface IAddPersonService
    {
        public Task<SavePersonResult> AddPerson(Person person);
    }
}
