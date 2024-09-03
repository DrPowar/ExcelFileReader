using Microsoft.EntityFrameworkCore;
using Server.DB;
using Server.Migrations;
using Server.Models.Log.Commands;
using Server.Models.Log.Queries;
using Server.Parser;

namespace Server.Models.Log.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ExcelDBContext _context;

        public LogRepository(ExcelDBContext excelDBContext)
        {
            _context = excelDBContext;
        }

        public async Task<LogsCommandResult> AddLog(Log log)
        {
            try
            {
                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();

                return new LogsCommandResult(true, ResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new LogsCommandResult(false, ex.Message);
            }
        }

        public async Task<LogsCommandResult> AddLogs(List<Log> logs)
        {
            try
            {
                await _context.Logs.AddRangeAsync(logs);
                await _context.SaveChangesAsync();

                return new LogsCommandResult(true, ResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new LogsCommandResult(false, ex.Message);
            }
        }

        public async Task<LogsCommandResult> DeleteLogs(List<Log> logs)
        {
            try
            {
                _context.Logs.RemoveRange(logs);
                await _context.SaveChangesAsync();

                return new LogsCommandResult(true, ResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new LogsCommandResult(false, ex.Message);
            }
        }

        public async Task<GetLogsResult> GetAllLogs()
        {
            try
            {
                List<Log> logs = await _context.Logs
                    .Include(log => log.Changes)
                    .ToListAsync();
                await _context.SaveChangesAsync();

                return new GetLogsResult(logs, ResultMessages.Success, true);
            }
            catch (Exception ex)
            {
                return new GetLogsResult(null!, ex.Message, false);
            }
        }
    }
}
