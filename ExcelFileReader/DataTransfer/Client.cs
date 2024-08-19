using ExcelFileReader.Constants;
using ExcelFileReader.Models;
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
                    return new ParsingResponse(new List<Person>(), false, ResponseMessages.ServerReturnInvalidData);
                }
            }
            catch
            {
                return new ParsingResponse(new List<Person>(), false, ResponseMessages.SendingFileError);
            }
        }

        internal async Task<SavingDataResponse> SaveDataOnServer(IEnumerable<Person> persons)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/SaveDataToDB", persons);

            SavingDataResponse? savingDataResponse = await response.Content.ReadFromJsonAsync<SavingDataResponse>();

            if (savingDataResponse != null)
            {
                return savingDataResponse;
            }
            else
            {
                return new SavingDataResponse(false, ResponseMessages.SendingFileError);
            }
        }
    }
}
