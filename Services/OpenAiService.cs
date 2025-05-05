using StudyHub.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace StudyHub.Services
{
    public class OpenAiService: IOpenAiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenAiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendRequest(string message)
        {
            using HttpClient client = _httpClientFactory.CreateClient("OpenAIClient");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
                new { role = "user", content = message }
            }
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenAI error: {response.StatusCode} - {responseString}");

            var json = JsonDocument.Parse(responseString);
            return json.RootElement
                       .GetProperty("choices")[0]
                       .GetProperty("message")
                       .GetProperty("content")
                       .GetString()!;
        }
    } 
}
