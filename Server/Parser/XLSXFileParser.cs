using IronXL;

namespace Server.Parser
{
    public static class XLSXFileParser
    {
        public static ParsingResult TryParseBook(FileUploadRequest fileUploadRequest, out WorkBook workBook)
        {
            try
            {
                WorkBook workbook = new WorkBook(fileUploadRequest.FileContent);
                workBook = workbook;
                return new ParsingResult(Guid.NewGuid(), true, fileUploadRequest.FileName, ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                workBook = null;
                return new ParsingResult(Guid.NewGuid(), false, fileUploadRequest.FileName, ex.Message);
            }
        }

    }
}
