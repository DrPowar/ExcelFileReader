namespace ExcelFileReader.DataTransfer
{
    internal record FileUploadRequest(string FileName, byte[] FileContent);
}
