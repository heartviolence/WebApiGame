using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Service;
using System.Linq;
using System.Security.Claims;

namespace SampleWebApi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CharacterController : ControllerBase
    {
        CharacterRepository _repository;
        ILogger _logger;
        public CharacterController(CharacterRepository repository, ILogger<CharacterController> logger)
        {
            this._repository = repository;
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

            var characters = await _repository.GetCharacters(userId);
            _logger.LogInformation("캐릭터 조회 성공 userId:{UserId}", userId);
            return Ok(characters);

        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            using (var context = new GameDbContext())
            {
                var user = context.UserInfos.Where(u => u.Id == userId)
                    .Include(u => u.Characters)
                    .FirstOrDefault();

                context.GameCharacters.RemoveRange(user.Characters);
                await context.SaveChangesAsync();
            }

            return Ok();
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

            await _repository.Gacha(userId);
            _logger.LogInformation("캐릭터 추가 성공 userId:{UserId}", userId);
            return Ok();
        }
    }
}
