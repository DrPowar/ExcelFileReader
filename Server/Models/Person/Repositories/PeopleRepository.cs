using Microsoft.EntityFrameworkCore;
using Server.DB;
using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;
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

        public async Task<PeopleCommandResult> AddPeople(List<Person> people)
        {
            try
            {
                await _context.Persons.AddRangeAsync(people);
                await _context.SaveChangesAsync();

                return new PeopleCommandResult(true, ResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new PeopleCommandResult(false, ex.Message);
            }
        }

        public async Task<PeopleCommandResult> DeletePeople(List<Person> people)
        {
            try
            {
                _context.Persons.RemoveRange(people);
                await _context.SaveChangesAsync();

                return new PeopleCommandResult(true, ResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new PeopleCommandResult(false, ex.Message);
            }
        }

        public async Task<GetPeopleResult> GetAllPeople()
        {
            try
            {
                List<Person> people = await _context.Persons.ToListAsync();
                await _context.SaveChangesAsync();

                return new GetPeopleResult(people, ResultMessages.Success, true);
            }
            catch(Exception ex)
            {
                return new GetPeopleResult(null!, ResultMessages.Success, false);
            }
        }

        public async Task<PeopleCommandResult> UpdatePeople(List<Person> people)
        {
            try
            {
                _context.Persons.UpdateRange(people);
                await _context.SaveChangesAsync();

                return new PeopleCommandResult(true, ResultMessages.Success);
            }
            catch(Exception ex)
            {
                return new PeopleCommandResult(false, ex.Message);
            }
        }
    }
}
