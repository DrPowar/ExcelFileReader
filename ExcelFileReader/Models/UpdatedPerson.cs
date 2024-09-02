namespace ExcelFileReader.Models
{
    internal class UpdatedPerson(Person person, Log log)
    {
        internal Person Person { get; set; } = person;
        internal Log Log { get; set; } = log;
    }
}
