using StudyHub.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using StudyHub.DTO;

namespace StudyHub.Services
{
    public class OpenAiService: IOpenAiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMessageService _messageService;

        public OpenAiService(IHttpClientFactory httpClientFactory, IMessageService messageService)
        {
            _httpClientFactory = httpClientFactory;
            _messageService = messageService;
        }

        public async Task<string> SendRequest(string message, string roomName)
        {
            using HttpClient client = _httpClientFactory.CreateClient("OpenAIClient");

            var requestBody = new
            {
                model = "gpt-4.1",
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
            var jsonContent = json.RootElement
                       .GetProperty("choices")[0]
                       .GetProperty("message")
                       .GetProperty("content")
                       .GetString()!;

            MessageDTO aiResponseMessage = new MessageDTO("ai", jsonContent, roomName);

            await _messageService.SaveMessage(aiResponseMessage);
            return jsonContent;
        }
    } 
}
