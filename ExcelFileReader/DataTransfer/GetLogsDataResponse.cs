using ExcelFileReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.DataTransfer
{
    internal record GetLogsDataResponse(List<Log> Logs, string Message, bool Result);
}
