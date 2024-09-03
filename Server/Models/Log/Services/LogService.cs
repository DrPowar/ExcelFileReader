using Server.Migrations;
using Server.Models.Log.Commands;
using Server.Models.Log.Queries;
using Server.Models.Log.Repositories;

namespace Server.Models.Log.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<LogsCommandResult> AddLog(Log log)
        {
            return await _logRepository.AddLog(log);
        }

        public async Task<LogsCommandResult> AddLogs(List<Log> logs)
        {
            return await _logRepository.AddLogs(logs);
        }

        public async Task<LogsCommandResult> DeleteLogs(List<Log> logs)
        {
            return await _logRepository.DeleteLogs(logs);
        }

        public async Task<GetLogsResult> GetAllLogs()
        {
            return await _logRepository.GetAllLogs();
        }
    }
}
