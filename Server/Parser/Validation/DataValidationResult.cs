using Server.Models.Person;

namespace Server.Parser.Validation
{
    public record DataValidationResult(bool Result, byte[] FileContent, string Message);
}
