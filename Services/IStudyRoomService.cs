using Microsoft.EntityFrameworkCore;
using StudyHub.Models;
using StudyHub.Utils;

namespace StudyHub.Services
{
    public interface IStudyRoomService
    {
        public Task<StudyRoomServiceResponse<string>> CreateRoom(string userId, string roomName);

        public Task<StudyRoomServiceResponse<bool>> DeleteRoom(string userId, string roomName);
    }
}
