using IronXL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Constants;
using Server.Parser.Validation;
using Server.Parser;
using Server.Models.Person;
using Server.Models.Log;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParserController : Controller
    {
        private readonly IMediator _mediator;
        public ParserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromBody] FileUploadRequest fileUploadRequest)
        {
            if (fileUploadRequest.FileContent == null)
            {
                return BadRequest(new ParsingFileToDataResult(Guid.NewGuid(), false, fileUploadRequest.FileName, ParsingResultMessages.EmptyFile));
            }

            WorkBook book;

            ParsingFileToDataResult parsingResult = XLSXFileParser.TryParseBook(fileUploadRequest, out book);

            FileValidatinResult validationResult = ExcelValidator.ValidateFile(book);

            if (validationResult.IsValid)
            {
                return Created("", validationResult);
            }
            else
            {
                return BadRequest(ParsingResultMessages.InvalidFile);
            }
        }


        [HttpPost("ParsePeopleToExcelFile")]
        public async Task<IActionResult> ParsePeopleToExcelFile([FromBody] List<Person> people)
        {
            if (people == null || people.Count == 0)
            {
                return BadRequest(new ParsingDataToFileResult(null, false, ParsingResultMessages.EmptyFile));
            }

            DataValidationResult validationResult = ExcelValidator.ValidatePeople(people);

            if (validationResult.Result)
            {
                return Created("", validationResult);
            }
            else
            {
                return BadRequest(ParsingResultMessages.InvalidFile);
            }
        }

        [HttpPost("ParseLogsToExcelFile")]
        public async Task<IActionResult> ParseLogsToExcelFile([FromBody] List<Log> logs)
        {
            if (logs == null || logs.Count == 0)
            {
                return BadRequest(new ParsingDataToFileResult(null, false, ParsingResultMessages.EmptyFile));
            }

            DataValidationResult validationResult = ExcelValidator.ValidateLogs(logs);

            if (validationResult.Result)
            {
                return Created("", validationResult);
            }
            else
            {
                return BadRequest(ParsingResultMessages.InvalidFile);
            }
        }
    }
}
