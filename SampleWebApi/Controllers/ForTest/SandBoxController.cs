using Assets.Scripts.Shared.GameDatas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Items;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;
using ServerShared.Shards;

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

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails
                    .Where(u => u.UserId == userId)
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
        public async Task<ActionResult> AddGrantItem()
        {
            var Items = new List<GameItem>();
            Items.Add(new GameItem() { Name = ItemNames.IAMATOMIC, Count = 1 });

            using (var context = new UserAccountDbContext())
            {
                context.GrantItems.Add(new GrantItem()
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
        public async Task<ActionResult> DeleteAllGrantItems()
        {
            using (var context = new UserAccountDbContext())
            {
                var targets = await context.GrantItems.Include(u => u.Items).ToListAsync();
                context.GrantItems.RemoveRange(targets);
                await context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
