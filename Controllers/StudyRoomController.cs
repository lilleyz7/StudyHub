using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyHub.Services;
using System.Security.Claims;

namespace StudyHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyRoomController : ControllerBase
    {
        private readonly StudyRoomService _studyRoomService;

        public StudyRoomController(StudyRoomService studyRoomService)
        {
            _studyRoomService = studyRoomService;
        }

        [Authorize]
        [HttpPost("/create-room")]
        public async Task<IActionResult> CreateRoom([FromBody] string roomName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var roomResult = await _studyRoomService.CreateRoom(userId, roomName);

            if (roomResult.data == null)
            {
                return BadRequest(roomResult.error);
            }

            return Ok(roomResult.data);
        }


    }
}
