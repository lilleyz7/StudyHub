namespace StudyHub.Models
{
    public class StudyRoom
    {
        public required string RoomName { get; set; }
        public required string CreatorId { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Dictionary<string, string> RoomMembers { get; set; } = new Dictionary<string, string>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
