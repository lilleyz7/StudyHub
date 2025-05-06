using Microsoft.EntityFrameworkCore;
using StudyHub.Data;
using StudyHub.DTO;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public class MessageService: IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HubResponse> SaveMessage(MessageDTO messageToAdd)
        {
            var room = _context.StudyRooms.FirstOrDefault(r => r.RoomName == messageToAdd.RoomName);
            if (room is null)
            {
                return new HubResponse(false, "room does not exist");
            }

            var message = new Message
            {
                RoomName = messageToAdd.RoomName,
                Room = room,
                Text = messageToAdd.Text,
                UserName = messageToAdd.UserName,
            };

            await _context.Messages.AddAsync(message);

            int addedSuccessfully = await _context.SaveChangesAsync();
            if (addedSuccessfully < 1)
            {
                return new HubResponse(false, "Failed to add message");
            }

            return new HubResponse(true, null);

        }

        //need to paginate
        public async Task<List<Message>?> GetMessages(string roomName)
        {
            var studyRoom = await _context.StudyRooms
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(m => m.RoomName == roomName);
            if (studyRoom is null)
            {
                return null;
            }

            return studyRoom.Messages.ToList();
        }

        public async Task<HubResponse> DeleteMessage(int messageId)
        {
            var messageToDelete = await _context.Messages.FindAsync(messageId);
            if (messageToDelete is null)
            {
                return new HubResponse(false, "message does not exist");
            }

            _context.Messages.Remove(messageToDelete);

            int changesMade = await _context.SaveChangesAsync();

            if (changesMade < 1)
            {
                return new HubResponse(false, "Failed to delete message");
            }

            return new HubResponse(true, null);
        }
    }
}
