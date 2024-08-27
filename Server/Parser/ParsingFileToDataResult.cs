using IronXL;

namespace Server.Parser
{
    public record ParsingFileToDataResult(Guid Id, bool IsValid, string FileName, string Message);
}
