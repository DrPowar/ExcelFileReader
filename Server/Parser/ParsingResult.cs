using IronXL;

namespace Server.Parser
{
    public record ParsingResult(Guid Id, bool IsValid, string FileName, string Message);
}
