using Assets.Scripts.Shared.GameDatas;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Model;
using SampleWebApi.Service.Games;
using ServerShared.DbContexts;
using ServerShared.Shards;
using System.Text.Json;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GameController : ControllerBase
    {
        ILogger _logger;
        public GameController(ILogger<GameController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<string> Start()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = new GameState();
                gamestate.Start(new List<string> { CharacterNames.Sora, CharacterNames.Nora });
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<string> SelectNPC(int index)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(user.GameState));
                gamestate.SelectNPC(index);
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<string> SelectCard(int index)
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(user.GameState));
                gamestate.SelectCard(index);
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<string> PowerUp()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(user.GameState));
                gamestate.BuyPowerUp();
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<string> NextFloor()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(user.GameState));
                gamestate.NextFloor();
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<string> BattleEnd()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
            {
                _logger.LogInformation("토큰에서 UserId를 찾지못함");
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails.Where(u => u.UserId == userId)
                    .FirstOrDefault();

                var gamestate = MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(user.GameState));
                gamestate.BattleEnd();
                user.GameState = Convert.ToBase64String(MessagePackSerializer.Serialize(gamestate));

                await context.SaveChangesAsync();
                return user.GameState;
            }
        }
    }
}
