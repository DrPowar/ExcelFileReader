using IronXL;
using Microsoft.AspNetCore.Mvc;
using Server.Parser;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : Controller
    {
        private List<WorkBook> _books = new List<WorkBook>();

        [HttpPost("UploadFile")]
        public IActionResult UploadFile([FromBody] FileUploadRequest fileUploadRequest)
        {
            if (fileUploadRequest.FileContent == null)
            {
                return BadRequest(new ParsingResult(Guid.NewGuid(), false, fileUploadRequest.FileName, ParsingResultMessages.EmptyFile));
            }

            WorkBook book = new WorkBook();

            ParsingResult parsingResult = XLSXFileParser.TryParseBook(fileUploadRequest, out book);

            if(parsingResult.IsValid)
            {
                _books.Add(book);
                return Created("", parsingResult);
            }
            else
            {
                return BadRequest(parsingResult);
            }
        }
    }

}
