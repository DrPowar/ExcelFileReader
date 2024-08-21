using BenchmarkDotNet.Attributes;
using IronXL;
using Server.Parser;
using Server.Parser.Validation;
using System.Diagnostics;
using System.Text;

namespace Server.Benchmark
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private string? excelFilePath = "C:\\Users\\User\\Desktop\\Files\\ExcelParser\\TestFiles\\file_example_XLSX_5000.xlsx";

        private WorkBook workBook;

        private FileUploadRequest fileUploadRequest;

        [GlobalSetup]
        public void GlobalSetup()
        {
            fileUploadRequest = GetFileInfo();
            XLSXFileParser.TryParseBook(fileUploadRequest, out workBook);
        }

        [Benchmark]
        public WorkBook GetWorkBook()
        {
            XLSXFileParser.TryParseBook(fileUploadRequest, out WorkBook book);
            return book;
        }

        [Benchmark]
        public ValidationResult ParseBook()
        {
            XLSXFileParser.TryParseBook(fileUploadRequest, out WorkBook book);

            ValidationResult vr = ExcelValidator.ValidateFile(book);   

            return vr;
        }

        private FileUploadRequest GetFileInfo()
        {
            if (!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException("Файл не знайдено", excelFilePath);
            }

            byte[] fileContent = File.ReadAllBytes(excelFilePath);

            string fileName = Path.GetFileName(excelFilePath);

            return new FileUploadRequest(fileName, fileContent);
        }
    }
}