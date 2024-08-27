using Server.Models.Person;

namespace Server.Parser.Validation
{
    public record FileValidatinResult(bool IsValid, List<Person>? People, string Message);
}
