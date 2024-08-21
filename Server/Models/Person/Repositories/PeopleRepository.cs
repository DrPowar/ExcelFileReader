using Server.DB;
using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Parser;
using System.Diagnostics;

namespace Server.Models.Person.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ExcelDBContext _context;

        public PeopleRepository(ExcelDBContext excelDBContext)
        {
            _context = excelDBContext;
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
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
