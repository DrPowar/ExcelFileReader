using MediatR;
using Server.Models.Person.Commands;

namespace Server.Models.Log.Queries
{
    public class GetAllLogsQuery : IRequest<GetLogsResult>
    {

    }
}
