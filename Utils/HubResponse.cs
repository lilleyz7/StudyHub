namespace StudyHub.Utils
{
    public class HubResponse(bool isSuccessful, string? error)
    {
        public bool isSuccessful { get; set; } = isSuccessful;
        public string? error { get; set; } = error;
    }
}
