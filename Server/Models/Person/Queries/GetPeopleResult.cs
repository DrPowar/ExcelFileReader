namespace Server.Models.Person.Queries
{
    public record GetPeopleResult(IEnumerable<Person> People, string Message, bool Result);
}
