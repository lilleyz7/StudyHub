﻿using Microsoft.AspNetCore.Authorization;
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
            await Clients.Group(roomName).SendAsync("UserJoined", userName + " has entered the chat");
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

        public Task LeaveRoomAsync(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task GetAiMessagesAsync(string roomName)
        {
            var messages = await _messageServive.GetAISpecificMessages(roomName);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveBulkMessages", messages);

        }

        public async Task<string?> SendAiRequestAsync(string prompt, string roomName)
        {
            try
            {
                var result = await _openAiService.SendRequest(prompt, roomName);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
