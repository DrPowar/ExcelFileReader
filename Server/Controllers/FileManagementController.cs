using IronXL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DB;
using Server.Models;
using Server.Parser;
using Server.Parser.Validation;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : Controller
    {
        ExcelDBContext _dbContext;
        public FileManagementController(ExcelDBContext dBContext) 
        {
            _dbContext = dBContext;
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

            if (validationResult.Result)
            {
                return Created("", validationResult.Persons);
            }
            else
            {
                return BadRequest(parsingResult);
            }
        }
    }
}
