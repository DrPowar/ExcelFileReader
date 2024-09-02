namespace Server.Models
{
    public class UpdatedPerson(Person.Person person, Log.Log log)
    {
        public Person.Person Person { get; set; } = person;
        public Log.Log Log { get; set; } = log;
    }
}
