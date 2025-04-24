namespace StudyHub.Utils
{
    public class StudyRoomServiceResponse<T>(T? data, string? error)
    {
        public T? data { get; set; } = data;
        public string? error { get; set; } = error;
    }
}
