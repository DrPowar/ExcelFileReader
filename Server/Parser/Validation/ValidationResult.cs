using Server.Models;

namespace Server.Parser.Validation
{
    internal record ValidationResult(bool Result, List<Person>? Persons);
}
