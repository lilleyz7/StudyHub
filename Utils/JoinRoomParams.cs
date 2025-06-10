using Scalar.AspNetCore;

namespace StudyHub.Utils
{
    public class JoinRoomParams
    {
        public required string UserId { get; set; }
        public required string RoomName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public JoinRoomParams(string userId, string roomname, string username, string password)
        {
            UserId = userId;
            RoomName = roomname;
            Username = username;
            Password = password;
        }
    }
}
