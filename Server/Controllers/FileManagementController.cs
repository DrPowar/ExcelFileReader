using IronXL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Constants;
using Server.Models.Person;
using Server.Models.Person.Commands;
using Server.Models.Person.Queries;
using Server.Parser;
using Server.Parser.Validation;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : Controller
    {
        private readonly IMediator _mediator;
        public FileManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromBody] FileUploadRequest fileUploadRequest)
        {
            if (fileUploadRequest.FileContent == null)
            {
                return BadRequest(new ParsingResult(Guid.NewGuid(), false, fileUploadRequest.FileName, ParsingResultMessages.EmptyFile));
            }

            WorkBook book;

            ParsingResult parsingResult = XLSXFileParser.TryParseBook(fileUploadRequest, out book);

            ValidationResult validationResult = ExcelValidator.ValidateFile(book);

            if (validationResult.IsValid)
            {
                return Created("", validationResult);
            }
            else
            {
                return BadRequest(ParsingResultMessages.InvalidFile);
            }
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
        public async Task<IActionResult> UpdateDataInDB([FromBody] List<Person> persons)
        {
            if (persons == null)
            {
                return BadRequest(new CommandDataResult(false, ResultMessages.NullData));
            }

            try
            {
                await _mediator.Send(new UpdatePeopleCommand(persons));

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

        [HttpGet("GetAllDataFromDB")]
        public async Task<IActionResult> GetAllDataFromDb()
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
