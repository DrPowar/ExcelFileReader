using IronXL;
using Server.Constants;
using Server.Models.Person;

namespace Server.Parser
{
    public static class XLSXFileParser
    {
        public static ParsingFileToDataResult TryParseBook(FileUploadRequest fileUploadRequest, out WorkBook workBook)
        {
            try
            {
                workBook = new WorkBook(fileUploadRequest.FileContent);
                return new ParsingFileToDataResult(Guid.NewGuid(), true, fileUploadRequest.FileName, ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                workBook = null;
                return new ParsingFileToDataResult(Guid.NewGuid(), false, fileUploadRequest.FileName, ex.Message);
            }
        }
    }
}
