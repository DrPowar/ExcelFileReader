using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Log;
using Server.Models.Log.Commands;
using Server.Models.Log.Queries;
using Server.Parser;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : Controller
    {
        private readonly IMediator _mediator;
        public LogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddLogs")]
        public async Task<IActionResult> AddLogs([FromBody] List<Log> logs)
        {
            if (logs == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new AddLogsCommand(logs));

                return Ok(new CommandDataResult(true, ResultMessages.Success));
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(new CommandDataResult(false, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new CommandDataResult(false, ex.Message));
            }
        }

        [HttpGet("GetLogs")]
        public async Task<IActionResult> GetLogs()
        {
            GetLogsResult response = await _mediator.Send(new GetAllLogsQuery());

            if (response.Result)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("DeleteLogsFromDB")]
        public async Task<IActionResult> DeleteLogsFromDB([FromBody] List<Log> logs)
        {
            if(logs == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new DeleteLogsCommand(logs));

                return Ok(new CommandDataResult(true, ResultMessages.Success));
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(new CommandDataResult(false, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new CommandDataResult(false, ex.Message));
            }

        }
    }
}
