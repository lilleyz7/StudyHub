namespace StudyHub.Services
{
    public interface IOpenAiService
    {
        public Task<string> SendRequest(string message, string roomName);
    }
}
