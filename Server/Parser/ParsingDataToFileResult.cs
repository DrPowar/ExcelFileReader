namespace Server.Parser
{
    public record ParsingDataToFileResult(byte[] FileContent, bool IsValid, string Message);
}
