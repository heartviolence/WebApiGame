using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Service;
using System.Security.Claims;

namespace SampleWebApi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CharacterController : ControllerBase
    {
        CharacterService _characterService;
        ILogger _logger;
        public CharacterController(CharacterService characterService, ILogger<CharacterController> logger)
        {
            this._characterService = characterService;
            this._logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GameCharacter>>> GetCharacters()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            var characters = await _characterService.GetCharacters(userId);
            _logger.LogInformation("캐릭터 조회 성공 userId:{UserId}", userId);
            return Ok(characters);

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Gacha()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            await _characterService.Gacha(userId);
            _logger.LogInformation("캐릭터 추가 성공 userId:{UserId}", userId);
            return Ok();
        }
    }
}
