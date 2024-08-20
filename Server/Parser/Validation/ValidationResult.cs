using Server.Models;

namespace Server.Parser.Validation
{
    internal record ValidationResult(bool IsValid, List<Person>? People, string Message);
}
