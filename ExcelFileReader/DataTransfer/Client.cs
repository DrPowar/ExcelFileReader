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
                return new SavingDataResponse(false, ResponseMessages.ServerReturnInvalidData);
            }
        }

        internal async Task<SavingDataResponse> DeleteDataOnServer(IEnumerable<Person> persons)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/DeletePeopleFromDB", persons);

            SavingDataResponse? savingDataResponse = await response.Content.ReadFromJsonAsync<SavingDataResponse>();

            if (savingDataResponse != null)
            {
                return savingDataResponse;
            }
            else
            {
                return new SavingDataResponse(false, ResponseMessages.ServerReturnInvalidData);
            }
        }

        internal async Task<SavingDataResponse> UpdateData(IEnumerable<Person> persons)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/UpdateDataInDB", persons);

            SavingDataResponse? savingDataResponse = await response.Content.ReadFromJsonAsync<SavingDataResponse>();

            if (savingDataResponse != null)
            {
                return savingDataResponse;
            }
            else
            {
                return new SavingDataResponse(false, ResponseMessages.ServerReturnInvalidData);
            }
        }

        internal async Task<GetAllDataResponse> GetAllDataFromDB()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/GetAllDataFromDB");

            GetAllDataResponse? getAllDataResponse = await response.Content.ReadFromJsonAsync<GetAllDataResponse>();

            return getAllDataResponse;
        }

        internal async Task<ParseDataToExcleFileResponse> ParseDataToExcelFile(IEnumerable<Person> persons)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/ParseDataToExcleFile", persons);

            ParseDataToExcleFileResponse? savingDataResponse = await response.Content.ReadFromJsonAsync<ParseDataToExcleFileResponse>();

            if (savingDataResponse != null)
            {
                return savingDataResponse;
            }
            else
            {
                return new ParseDataToExcleFileResponse(null, false, ResponseMessages.ServerReturnInvalidData);
            }
        }
    }
}
