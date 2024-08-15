namespace Server.Parser
{
    public record FileUploadRequest(string FileName, byte[] FileContent);
}
