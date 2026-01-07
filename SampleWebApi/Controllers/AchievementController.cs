using Assets.Scripts.Shared;
using Assets.Scripts.Shared.GameDatas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Controllers.ForTest;
using SampleWebApi.Model.Items;
using SampleWebApi.Service.Achievements;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AchievementController : ControllerBase
    {
        ILogger _logger;
        AchievementRepository _repository;
        public AchievementController(ILogger<AchievementController> logger, AchievementRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> GainAcheivementRewards(string achievementName)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            await _repository.GainAcheivementRewards(userId, achievementName);

            return Ok();
        }
    }
}
