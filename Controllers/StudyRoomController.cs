﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudyHub.DTO;
using StudyHub.Services;
using System.Security.Claims;
using BCrypt;
using StudyHub.Utils;

namespace StudyHub.Controllers
{
    [EnableRateLimiting("production")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudyRoomController : ControllerBase
    {
        private readonly IStudyRoomService _studyRoomService;

        public StudyRoomController(IStudyRoomService studyRoomService)
        {
            _studyRoomService = studyRoomService;
        }

        [Authorize]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom(RoomDTO roomData)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            bool nameIsAvailable = await _studyRoomService.IsNameAvailble(roomData.roomName);
            if (!nameIsAvailable)
            {
                return Conflict("Room already taken");
            }

            var roomResult = await _studyRoomService.CreateRoom(userId, roomData.roomName, roomData.username, roomData.password);

            if (roomResult.data == null)
            {
                return BadRequest(roomResult.error);
            }

            return Ok(roomResult.data);
        }

        [Authorize]
        [HttpPost("/join-room")]
        public async Task<IActionResult> JoinRoom(JoinRoomParams parameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var serviceResponse = await _studyRoomService.JoinRoom(parameters);
            if (!serviceResponse.data)
            {
                return BadRequest(serviceResponse.error);
            }

            return Ok("Successfully Joined");
        }

        [Authorize]
        [HttpDelete("delete-room/{roomName}")]
        public async Task<IActionResult> DeleteRoom(string roomName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var deleteRoomResult =  await _studyRoomService.DeleteRoom(userId, roomName);

            if (deleteRoomResult.data == false)
            {
                return BadRequest(deleteRoomResult.error);
            }

            return Ok(deleteRoomResult.data);

        }



    }
}
