using Microsoft.EntityFrameworkCore;
using StudyHub.Data;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public class StudyRoomService : IStudyRoomService
    {
        private readonly ApplicationDbContext _context;
        public StudyRoomService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<StudyRoomServiceResponse<string>> CreateRoom(string userId, string roomName)
        {
            var user = await _context.Users.Include(p => p.rooms).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new StudyRoomServiceResponse<string>(null, "user does not exist");
            }

            if (user.rooms.Count >= 5){
                return new StudyRoomServiceResponse<string>(null, "Sorry you have the maximum amount of rooms");
            }

            bool roomExists = user.rooms.Any(r => r.RoomName == roomName);
            if (roomExists)
                return new StudyRoomServiceResponse<string>(null, "Room name already exists.");

            var room = new StudyRoom { CreatorId = userId, RoomName = roomName };
            await _context.StudyRooms.AddAsync(room);

            int isSuccessful = await _context.SaveChangesAsync();
            if (isSuccessful == 0)
            {
                return new StudyRoomServiceResponse<string>(null, "Unable to save changes");
            }

            return new StudyRoomServiceResponse<string>(roomName, null);
        } 

        public async Task<StudyRoomServiceResponse<bool>> DeleteRoom(string userId, string roomName)
        {
            var room = await _context.StudyRooms.FirstOrDefaultAsync(s => s.RoomName == roomName);
            if (room is null)
            {
                return new StudyRoomServiceResponse<bool>(false, "Invalid room");
            }

            if (room.CreatorId != userId)
            {
                return new StudyRoomServiceResponse<bool>(false, "Incorrect owner");
            }

            _context.StudyRooms.Remove(room);
            int isSuccessful = await _context.SaveChangesAsync();
            if(isSuccessful < 1)
            {
                return new StudyRoomServiceResponse<bool>(false, "Unable to remove room");
            }

            return new StudyRoomServiceResponse<bool>(true, null);
        }

        public async Task<bool> IsNameAvailble(string roomName)
        {
            StudyRoom? room = await _context.StudyRooms.FirstOrDefaultAsync(r => r.RoomName == roomName);
            if (room is null)
            {
                return true;
            }

            return false;
        }
    }
}
