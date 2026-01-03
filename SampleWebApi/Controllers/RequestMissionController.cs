using Assets.Scripts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Service;
using System.Text.Json;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RequestMissionController : ControllerBase
    {
        RequestMissionRepository _repository;
        ILogger _logger;
        public RequestMissionController(RequestMissionRepository repository, ILogger<RequestMissionController> logger)
        {
            this._repository = repository;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<bool>> StartMission(RequestMissionStartRequest request)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            if (await _repository.StartMission(userId, request.MissionCode, request.CharacterCode))
            {
                _logger.LogInformation("미션 시작 성공 userId:{UserId},request:{Request}", userId, JsonSerializer.Serialize(request));
                return Ok(true);
            }
            else
            {
                _logger.LogInformation("미션 시작 실패 userId:{UserId},request:{Request}", userId, JsonSerializer.Serialize(request));
                return Ok(false);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> RequestMissionCompleteCheck()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            await _repository.RequestMissionCompleteCheck(userId);
            _logger.LogInformation("미션 정보 갱신완료 userId:{UserId}", userId);
            return Ok();
        }
    }
}