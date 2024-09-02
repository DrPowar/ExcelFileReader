namespace Server.Models.Log
{
    public sealed class Log
    {
        public Guid Id { get; set; }

        public uint PersonNumber { get; set; }

        public Person.Person? OldValue { get; set; }

        public Person.Person? NewValue { get; set; }

        public DateTime Date { get; set; }
    }
}
