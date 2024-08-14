using IronXL;
using IronXL.Options;
using Microsoft.AspNetCore.Mvc;
using Server.Parser;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : Controller
    {
        private List<byte[]> files = new List<byte[]>();

        [HttpPost("UploadFile")]
        public IActionResult UploadFile([FromBody] byte[] file)
        {
            if (file == null)
            {
                return BadRequest("Invalid file data.");
            }

            files.Add(file);

            XLSXFileParser.ParseBook(file);

            return Created("", file);
        }
    }

}
