using MediatR;
using Server.Models.Person.Commands;

namespace Server.Models.Person.Queries
{
    public class GetAllPeopleQuery : IRequest<GetPeopleResult>
    {

    }
}
