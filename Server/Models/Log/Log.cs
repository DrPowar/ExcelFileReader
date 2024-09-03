namespace Server.Models.Log
{
    public sealed class Log
    {
        public Log()
        {
        }

        public Log(Guid id, uint personNumber, OldNewValuePair changes, DateTime date)
        {
            Id = id;
            PersonNumber = personNumber;
            Changes = changes;
            Date = date;
        }

        public Guid Id { get; set; }

        public uint PersonNumber { get; set; }

        public OldNewValuePair Changes { get; set; }

        public DateTime Date { get; set; }
    }

}
