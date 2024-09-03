namespace Server.Models.Log.Queries
{
    public record GetLogsResult(IEnumerable<Log> Logs, string Message, bool Result);
}
