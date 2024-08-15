using System;

namespace ExcelFileReader.DataTransfer
{
    internal record FileParsingResponse(Guid Id, bool IsValid, string FileName, string Message);
}
