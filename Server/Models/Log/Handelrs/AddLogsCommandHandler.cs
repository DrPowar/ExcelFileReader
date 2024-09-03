using MediatR;
using Server.Models.Log.Commands;
using Server.Models.Log.Services;

namespace Server.Models.Log.Handlers
{
    public class AddLogsCommandHandler : IRequestHandler<AddLogsCommand, LogsCommandResult>
    {
        private readonly ILogService _logService;

        public AddLogsCommandHandler(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<LogsCommandResult> Handle(AddLogsCommand addLogsCommand, CancellationToken cancellationToken)
        {
            var result = await _logService.AddLogs(addLogsCommand.Logs);
            return result;
        }
    }
}
