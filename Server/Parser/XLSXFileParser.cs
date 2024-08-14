using IronXL.Options;
using IronXL;

namespace Server.Parser
{
    public static class XLSXFileParser
    {
        public static string ParseBook(byte[] file)
        {
            string tempFilePath = "C:\\Users\\User\\Desktop\\Files\\ExcelParser\\TestFiles\\serverFile";

            try
            {
                File.WriteAllBytes(tempFilePath, file);
            }
            finally
            {
            }

            return "dsgf";
        }

    }
}
