using IronXL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DB;
using Server.Models;
using Server.Parser;

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

            if(parsingResult.IsValid && !IsFileInDB(parsingResult.FileName))
            {
                _dbContext.Files.Add(new ExcelFile(parsingResult.Id, parsingResult.FileName, fileUploadRequest.FileContent));
                await _dbContext.SaveChangesAsync();
                return Created("", parsingResult);
            }
            else if(IsFileInDB(parsingResult.FileName) && parsingResult.IsValid)
            {
                return Created("", new ParsingResult(parsingResult.Id, true, parsingResult.FileName, ParsingResultMessages.FileInDatabase));
            }
            else
            {
                return BadRequest(parsingResult);
            }
        }

        private bool IsFileInDB(string name) =>
            _dbContext.Files.Any(file => file.Name == name);
    }
}
