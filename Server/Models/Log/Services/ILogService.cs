using Server.Models.Log.Commands;
using Server.Models.Log.Queries;

namespace Server.Models.Log.Services
{
    public interface ILogService
    {
        public Task<LogsCommandResult> AddLogs(List<Log> logs);
        public Task<LogsCommandResult> DeleteLogs(List<Log> logs);
        public Task<GetLogsResult> GetAllLogs();
    }
}
