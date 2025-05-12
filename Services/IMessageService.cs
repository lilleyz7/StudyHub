using StudyHub.DTO;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public interface IMessageService
    {
        public Task<HubResponse> SaveMessage(MessageDTO messageToAdd);
        public Task<List<Message>?> GetMessages(string roomName);
        public Task<List<Message>?> GetAISpecificMessages(string roomName);
        public Task<HubResponse> DeleteMessage(int messageId);
    }
}
