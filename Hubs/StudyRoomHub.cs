using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using StudyHub.Models;
using StudyHub.Data;
using StudyHub.DTO;
using StudyHub.Services;

namespace StudyHub.Hubs
{
    public class StudyRoomHub: Hub
    {
        private readonly MessageService _service;

        public StudyRoomHub(MessageService service)
        {
            _service = service;
        }
        public async Task JoinRoomAsync(string userName, string roomName)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync(userName + " has entered the chat");
        }

        public async Task SendMessageAsync(string roomName, string userName, string message)
        {
            MessageDTO messageToAdd = new MessageDTO(userName: userName, text: message, roomName: roomName);
            var saveSuccess = await _service.SaveMessage(messageToAdd);

            if (saveSuccess.isSuccessful)
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", userName, message);
            }
            else
            {
                await Clients.Group(roomName).SendAsync("ErrorMessage", userName, saveSuccess.error);
            }
        }

        public Task LeaveRoomAsync(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
