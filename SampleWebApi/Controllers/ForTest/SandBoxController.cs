using Assets.Scripts.Shared.GameDatas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Service.Snapshots;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Events.SandBox;
using ServerShared.Shards;
using ServerShared.Util;

namespace SampleWebApi.Controllers.ForTest
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SandBoxController : ControllerBase
    {
        ILogger _logger;
        GameItemService _gameItemService;
        SnapshotRollback _userSnapshot;
        public SandBoxController(ILogger<SandBoxController> logger, GameItemService gameItemService, SnapshotRollback userSnapshot)
        {
            this._logger = logger;
            this._gameItemService = gameItemService;
            this._userSnapshot = userSnapshot;
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

                var beforeCrystal = _gameItemService.AddItem(user, ItemNames.Crystal, 100);
                var beforelevelupMat = _gameItemService.AddItem(user, ItemNames.CharacterLevelUpMaterial, 10);
                var beforeRankupMat = _gameItemService.AddItem(user, ItemNames.CharacterRankUpMaterial, 10);

                var gameEvent = new ShowMetheMoneyEvent()
                {
                    UserId = userId,
                    ModifiedItems = new()
                    {
                        new ModifiedItemCountInfo() { ItemName = ItemNames.Crystal, BeforeCount = beforeCrystal, AfterCount=user.Crystal().Count},
                        new ModifiedItemCountInfo() { ItemName = ItemNames.CharacterLevelUpMaterial, BeforeCount = beforelevelupMat,AfterCount = user.GameItem(ItemNames.CharacterLevelUpMaterial).Count  },
                        new ModifiedItemCountInfo() { ItemName = ItemNames.CharacterRankUpMaterial, BeforeCount = beforeRankupMat,AfterCount = user.GameItem(ItemNames.CharacterRankUpMaterial).Count}
                    }
                };
                context.GameEvents.Add(gameEvent.CovertToGameEvent());
                user.RowVersion = Guid.NewGuid();
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

        [HttpPost]
        public async Task<ActionResult> SnapshotReplayTest()
        {
            await _userSnapshot.RollbackToSnapshot(5);

            return Ok();
        }


    }
}
