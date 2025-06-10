using Microsoft.EntityFrameworkCore;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public interface IStudyRoomService
    {
        public Task<StudyRoomServiceResponse<string>> CreateRoom(string userId, string roomName, string username, string password);

        public Task<StudyRoomServiceResponse<bool>> DeleteRoom(string userId, string roomName);
        public Task<StudyRoomServiceResponse<bool>> JoinRoom(JoinRoomParams parameters);
        public Task<bool> IsNameAvailble(string roomName);
    }
}
