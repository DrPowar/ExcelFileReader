using System;

namespace ExcelFileReader.Models
{
    public class ExcelFileResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public string FileName { get; set; }

        public ExcelFileResponse(Guid id, string errorMessage, string fileName, bool isValid)
        {
            Id = id;
            Message = errorMessage;
            IsValid = isValid;
            FileName = fileName;
        }

    }
}