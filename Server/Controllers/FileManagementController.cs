using IronXL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Constants;
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
            if(persons == null)
            {
                return BadRequest(new SavingDataResult(false, SavingResultMessages.NullData));
            }

            try
            {
                await _dbContext.Persons.AddRangeAsync(persons);
                await _dbContext.SaveChangesAsync();

                return Ok(new SavingDataResult(true, SavingResultMessages.Success));
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(new SavingDataResult(false, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new SavingDataResult(false, ex.Message));
            }
        }
    }
}
