using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Model;
using SampleWebApi.Service.Characters;

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


        [HttpPost]
        [Authorize]
        public async Task<GameCharacterDTO> UseLevelUpItem(string characterName, int itemCount)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return null;
            }

            var characterData = await _repository.UseLevelUpItem(userId, characterName, itemCount);
            return DTOConverter.DTO(characterData);
        }

        [HttpPost]
        [Authorize]
        public async Task<GameCharacterDTO> RankUp(string characterName)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return null;
            }

            var characterData = await _repository.RankUp(userId, characterName);
            return characterData is null ? null : DTOConverter.DTO(characterData);
        }
    }
}
