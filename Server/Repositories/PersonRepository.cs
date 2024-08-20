using Server.Commands;
using Server.DB;
using Server.Models;
using Server.Parser;

namespace Server.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ExcelDBContext _context;

        public PersonRepository(ExcelDBContext excelDBContext)
        {
            _context = excelDBContext;
        }

        public async Task<SavePersonResult> AddPerson(Person person)
        {
            try
            {
                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();
                return new SavePersonResult(true, SavingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new SavePersonResult(false, ex.Message);
            }
        }

        public async Task<SavePeopleResult> AddPeople(List<Person> people)
        {
            try
            {
                await _context.Persons.AddRangeAsync(people);
                await _context.SaveChangesAsync();
                return new SavePeopleResult(true, SavingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new SavePeopleResult(false, ex.Message);
            }
        }
    }
}
