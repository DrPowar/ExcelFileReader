﻿using Server.Models.Log.Commands;
using Server.Models.Log.Queries;

namespace Server.Models.Log.Repositories
{
    public interface ILogRepository
    {
        public Task<LogsCommandResult> AddLogs(List<Log> logs);
        public Task<LogsCommandResult> DeleteLogs(List<Log> logs);
        public Task<LogsCommandResult> AddLog(Log log);

        public Task<GetLogsResult> GetAllLogs();
    }
}
