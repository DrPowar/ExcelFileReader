namespace Server.Models.Log.Queries
{
    internal record GetLogsResult(IEnumerable<Log> People, string Message, bool Result);
}
