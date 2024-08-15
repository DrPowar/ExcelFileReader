using ExcelFileReader.Constants;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExcelFileReader.DataTransfer
{
    internal class Client
    {
        private HttpClient _httpClient;

        internal Client()
        {
            _httpClient = new HttpClient();
        }

        internal async Task<FileParsingResponse> SendFile(byte[] fileContent, string fileName)
        {
            try
            {
                FileUploadRequest fileUploadRequest = new FileUploadRequest(fileName, fileContent);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/UploadFile", fileUploadRequest);

                return await response.Content.ReadFromJsonAsync<FileParsingResponse>();
            }
            catch
            {
                return new FileParsingResponse(Guid.NewGuid(), false, fileName, ParsingResultMessages.SendingFileError);
            }
        }
    }
}
