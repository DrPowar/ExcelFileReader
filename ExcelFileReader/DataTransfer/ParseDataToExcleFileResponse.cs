namespace ExcelFileReader.DataTransfer
{
    internal record ParseDataToExcleFileResponse(byte[] FileContent, bool Result, string Message);
}
