namespace Server.Models.Log
{
    public sealed class Log
    {
        public Log()
        {
        }

        public Log(Guid id, uint personNumber, OldNewValuePair changes, LogActions action, DateTime date)
        {
            Id = id;
            PersonNumber = personNumber;
            Changes = changes;
            Action = action;
            Date = date;
        }

        public Guid Id { get; set; }

        public uint PersonNumber { get; set; }

        public OldNewValuePair Changes { get; set; }

        public LogActions Action { get; set; }

        public DateTime Date { get; set; }
    }

}
