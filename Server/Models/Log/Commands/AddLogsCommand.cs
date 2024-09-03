using MediatR;

namespace Server.Models.Log.Commands
{
    public class AddLogsCommand : IRequest<LogsCommandResult>
    {
        public List<Log> Logs { get; private set; }

        public AddLogsCommand(List<Log> logs)
        {
            Logs = logs;
        }
    }
}
