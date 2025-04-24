namespace StudyHub.Models
{
    public class StudyRoom
    {
        public required string RoomName { get; set; }
        public required string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
