using StudyHub.DTO;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public interface IMessageService
    {
        public Task<HubResponse> SaveMessage(MessageDTO messageToAdd);
    }
}
