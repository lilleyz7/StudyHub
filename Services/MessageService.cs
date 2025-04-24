using StudyHub.Data;
using StudyHub.DTO;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public class MessageService
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
    }
}
