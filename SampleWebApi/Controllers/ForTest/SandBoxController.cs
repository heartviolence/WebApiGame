using Assets.Scripts.Shared.GameDatas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Items;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;

namespace SampleWebApi.Controllers.ForTest
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SandBoxController : ControllerBase
    {
        ILogger _logger;
        GameItemService _gameItemService;
        public SandBoxController(ILogger<SandBoxController> logger, GameItemService gameItemService)
        {
            this._logger = logger;
            this._gameItemService = gameItemService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ShowMeTheMoney()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
                return BadRequest();
            }

            using (var context = new GameDbContext())
            {
                var user = context.UserInfos
                    .Where(u => u.Id == userId)
                    .Include(u => u.GameItems)
                    .SingleOrDefault();

                _gameItemService.AddItem(user, SpeicalItemNames.Crystal, 100);
                _gameItemService.AddItem(user, ItemNames.CharacterLevelUpMaterial, 10);
                _gameItemService.AddItem(user, ItemNames.CharacterRankUpMaterial, 10);

                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddUserReward()
        {
            var Items = new List<GameItem>();
            Items.Add(new GameItem() { Name = ItemNames.IAMATOMIC, Count = 1 });

            using (var context = new GameDbContext())
            {
                context.UserRewards.Add(new UserReward()
                {
                    Name = "아이템증정이벤트",
                    Description = "",
                    Items = Items,
                    ExpireTime = DateTime.Now + TimeSpan.FromSeconds(30)
                });

                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllUserReward()
        {
            using (var context = new GameDbContext())
            {
                var targets = await context.UserRewards.Include(u => u.Items).ToListAsync();
                context.UserRewards.RemoveRange(targets);
                await context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
