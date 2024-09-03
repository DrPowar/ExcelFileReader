using MediatR;
using Server.Models.Log.Queries;
using Server.Models.Log.Services;

namespace Server.Models.Log.Handlers
{
    public class GetAllLogsQueryHandler : IRequestHandler<GetAllLogsQuery, GetLogsResult>
    {
        private readonly ILogService _logService;

        public GetAllLogsQueryHandler(ILogService logService)
        {
            _logService = logService;
        }
        public async Task<GetLogsResult> Handle(GetAllLogsQuery request, CancellationToken cancellationToken)
        {
            GetLogsResult logs = await _logService.GetAllLogs();

            return logs;
        }
    }
}
