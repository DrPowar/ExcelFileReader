using Server.Models.Person;

namespace Server.Parser.Validation
{
    public record ValidationResult(bool IsValid, List<Person>? People, string Message);
}
