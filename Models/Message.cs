namespace StudyHub.Models
{
    public class Message
    {
        public required string UserName { get; set; }
        public required string Text { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public required string RoomName { get; set; } 
        public required StudyRoom Room { get; set; }   
    }
}
