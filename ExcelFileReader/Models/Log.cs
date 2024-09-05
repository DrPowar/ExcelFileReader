using Avalonia.Data;
using System;
using System.Collections.Generic;

namespace ExcelFileReader.Models
{
    public sealed class Log
    {
        public Log()
        {
        }

        public Log(uint personNumber, OldNewValuePair changes, LogActions action)
        {
            Id = Guid.NewGuid();
            PersonNumber = personNumber;
            Changes = changes;
            Action = action;
            Date = DateTime.Now;
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
