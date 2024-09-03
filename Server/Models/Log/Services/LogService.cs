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

        public async Task<LogsCommandResult> AddLogs(List<Log> people)
        {
            return await _logRepository.AddLogs(people);
        }

        public async Task<GetLogsResult> GetAllLogs()
        {
            return await _logRepository.GetAllLogs();
        }
    }
}
