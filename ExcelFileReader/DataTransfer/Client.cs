using ExcelFileReader.Constants;
using System;
using System.Collections.Generic;
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
            _httpClient = HttpClientFactory.Create();
        }

        internal async Task<ParsingResponse> SendFile(byte[] fileContent, string fileName)
        {
            try
            {
                FileUploadRequest fileUploadRequest = new FileUploadRequest(fileName, fileContent);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/UploadFile", fileUploadRequest);

                ParsingResponse? fileParsingResponse = await response.Content.ReadFromJsonAsync<ParsingResponse>();
                    
                if(fileParsingResponse != null)
                {
                    return fileParsingResponse;
                }
                else
                {
                    return new ParsingResponse(new List<Models.Person>(), false, ParsingResultMessages.ServerReturnInvalidData);
                }
            }
            catch
            {
                return new ParsingResponse(new List<Models.Person>(), false, ParsingResultMessages.SendingFileError);
            }
        }
    }
}
