using System;

namespace ExcelFileReader.Models
{
    public sealed class ExcelError
    {
        public Guid Id { get; set; }
        public string ErrorMessage { get; set; }

        public ExcelError(string errorMessage)
        {
            Id = Guid.NewGuid();
            ErrorMessage = errorMessage;
        }

    }
}