using Assets.Scripts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model;
using SampleWebApi.Service.Users;
using ServerShared.DbContexts;
using ServerShared.Shards;

namespace SampleWebApi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        UserRepository _repository;
        ILogger _logger;
        public UserController(UserRepository repository, ILogger<UserController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GameCharacterDTO>>> GetCharacters()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            var characters = await _repository.GetCharacters(userId);
            _logger.LogInformation("캐릭터 조회 성공 userId:{UserId}", userId);
            return Ok(characters.ConvertAll(x => x.DTO()));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserInfoDTO>> GetUserInfo()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            var userInfo = await _repository.GetUserInfo(userId);
            if(userInfo == null)
            { 
                _logger.LogInformation("유저데이터를 찾지못함 userId:{UserId}", userId);
                return NotFound();
            }
            _logger.LogInformation("유저데이터 조회 성공 userId:{UserId}", userId);
            return Ok(userInfo.DTO());
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

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
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
