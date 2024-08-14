using ExcelFileReader.Constants;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ExcelFileReader.DataTransfer
{
    internal class Client
    {
        private HttpClient _httpClient;

        internal Client()
        {
            _httpClient = new HttpClient();
        }

        internal async Task<bool> SendFile(string fileContent)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"http://localhost:{ServerData.ServerPort}/FileManagement/UploadFile", byteArray);
                string a = response.StatusCode.ToString();

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
