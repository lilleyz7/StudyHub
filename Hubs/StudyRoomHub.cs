using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using StudyHub.Models;
using StudyHub.Data;
using StudyHub.DTO;
using StudyHub.Services;
using Microsoft.AspNetCore.RateLimiting;


namespace StudyHub.Hubs
{
    [EnableRateLimiting("production")]
    public class StudyRoomHub: Hub
    {
        private readonly IMessageService _messageServive;
        private readonly IOpenAiService _openAiService;

        public StudyRoomHub(IMessageService service, IOpenAiService openAiService)
        {
            _messageServive = service;
            _openAiService = openAiService;
        }
        public async Task JoinRoomAsync(string userName, string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("UserJoined", userName);
            List<Message>? messages = await _messageServive.GetMessages(roomName);
            if(messages is null)
            {
                return;
            }

            List<MessageDTO> previousMessages = new List<MessageDTO>();
            foreach (var message in messages) {

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message.UserName, message.Text);
            }

        }

        public async Task SendMessageAsync(string roomName, string userName, string message)
        {
            MessageDTO messageToAdd = new MessageDTO(userName: userName, text: message, roomName: roomName);
            var saveSuccess = await _messageServive.SaveMessage(messageToAdd);

            if (saveSuccess.isSuccessful)
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", userName, message);
            }
            else
            {
                await Clients.Group(roomName).SendAsync("ErrorMessage", userName, saveSuccess.error);
            }
        }

        [DisableRateLimiting]
        public Task LeaveRoomAsync(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        //public async Task GetRoomMessages(string roomName)
        //{
        //    var messages = await _messageServive.GetMessages(roomName);
        //    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveBulkMessages", messages);

        //}

        public async Task SendAiRequestAsync(string roomName, string inputContent)
        {
            String prompt = $"You are an ai agent for helping students study. Can you answer the following question if it is appropriate: {inputContent}";
            try
            {
                var result = await _openAiService.SendRequest(prompt, roomName);
                await Clients.Group(roomName).SendAsync("ReceiveMessage", "ai", result);
            }
            catch (Exception ex)
            {
                await Clients.Group(roomName).SendAsync("ErrorMessage", "error", ex + "");
            }
            
        }
    }
}
