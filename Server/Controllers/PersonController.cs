using IronXL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Constants;
using Server.Models;
using Server.Models.Log.Queries;
using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;
using Server.Parser;
using Server.Parser.Validation;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : Controller
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveDataToDB")]
        public async Task<IActionResult> SaveDataToDB([FromBody] List<Person> persons)
        {
            if (persons == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new AddPeopleCommand(persons));

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

        [HttpPost("DeletePeopleFromDB")]
        public async Task<IActionResult> DeletePeopleFromDB([FromBody] List<Person> persons)
        {
            if (persons == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new DeletePeopleCommand(persons));

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

        [HttpPost("UpdateDataInDB")]
        public async Task<IActionResult> UpdateDataInDB([FromBody] List<Person> updatePeople)
        {
            if (updatePeople == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new UpdatePeopleCommand(updatePeople));

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

        [HttpGet("GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            GetPeopleResult response = await _mediator.Send(new GetAllPeopleQuery());

            if (response.Result)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
