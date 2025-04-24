namespace StudyHub.DTO
{
    public class MessageDTO(string userName, string text, string roomName)
    {
        public string UserName { get; set; } = userName;
        public string Text { get; set; } = text;
        public string RoomName { get; set; } = roomName;

    }
}
