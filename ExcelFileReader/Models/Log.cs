using Avalonia.Data;
using System;
using System.Collections.Generic;

namespace ExcelFileReader.Models
{
    internal sealed class Log
    {
        internal Log(uint personNumber, List<OldNewValuePair> changes)
        {
            Id = Guid.NewGuid();
            PersonNumber = personNumber;
            Changes = changes;
            Date = DateTime.Now;
        }

        internal Guid Id { get; set; }

        internal uint PersonNumber { get; set; }

        internal List<OldNewValuePair> Changes { get; set; }

        internal DateTime Date { get; set; }
    }
}
