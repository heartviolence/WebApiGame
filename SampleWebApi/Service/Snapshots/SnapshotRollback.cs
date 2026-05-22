using Assets.Scripts.Shared.GameDatas;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model;
using SampleWebApi.Model.Characters;
using SampleWebApi.Service.Achievements;
using SampleWebApi.Service.Characters;
using SampleWebApi.Service.RequestMissions;
using SampleWebApi.Service.Users;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Events.SandBox;
using ServerShared.Shards;
using ServerShared.Util;
using System.Diagnostics;
using System.Text.Json;

namespace SampleWebApi.Service.Snapshots
{
    public class SnapshotRollback
    {
        CharacterService _characterService;
        AchievementService _achievementService;
        RequestMissionService _requestMissionService;
        UserRepository _userRepository;
        ILogger _logger;
        public SnapshotRollback(
            CharacterService characterService,
            AchievementService achievementService,
            RequestMissionService requestMissionService,
            UserRepository userRepository,
            ILogger<SnapshotRollback> logger)
        {
            this._characterService = characterService;
            this._achievementService = achievementService;
            this._requestMissionService = requestMissionService;
            this._userRepository = userRepository;
            this._logger = logger;
        }

        public async Task RollbackToSnapshot(int userId)
        {
            using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var lastSnapshot = await context.GameEvents
                    .Where(e => e.UserId == userId && e.EventType == nameof(UserSnapshotEvent))
                    .OrderBy(e => e.Id)
                    .LastAsync();

                var afterGameEvents = await context.GameEvents
                    .Where(e => e.UserId == userId && e.Id > lastSnapshot.Id)
                    .ToListAsync();

                var snapshotEvent = JsonSerializer.Deserialize<UserSnapshotEvent>(lastSnapshot.Payload);
                var beforeUserData = await context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.AchievementData)
                    .Include(u => u.RequestMissions)
                    .Include(u => u.GameItems)
                    .Include(u => u.Records)
                    .Include(u => u.CompletedAchievements)
                    .Include(u => u.MailBox)
                    .ThenInclude(m => m.Items)
                    .Include(u => u.ReceievedGrantItem)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();
                var beforeUserDataJson = JsonSerializer.Serialize(beforeUserData);
                PlayGameEvents(snapshotEvent.UserData, afterGameEvents);

                var newData = snapshotEvent.UserData;
                var afterUserData = JsonSerializer.Serialize(newData);
                _logger.LogInformation("Before: {BeforeUserData}\n\n\nAfter: {AfterUserData}", beforeUserDataJson, afterUserData);

                beforeUserData.Characters.Clear();
                foreach (var character in newData.Characters)
                {
                    beforeUserData.Characters.Add(character);
                }

                context.GameItems.RemoveRange(beforeUserData.GameItems);
                foreach (var gameItem in newData.GameItems)
                {
                    beforeUserData.GameItems.Add(gameItem);
                }

                beforeUserData.AchievementData.GachaCount = newData.AchievementData.GachaCount;

                context.GameItems.RemoveRange(beforeUserData.MailBox.SelectMany(m => m.Items));
                beforeUserData.MailBox.Clear();
                foreach (var mail in newData.MailBox)
                {
                    beforeUserData.MailBox.Add(mail);
                }

                beforeUserData.RequestMissions.Clear();
                foreach (var requestMission in newData.RequestMissions)
                {
                    beforeUserData.RequestMissions.Add(requestMission);
                }

                beforeUserData.ReceievedGrantItem.Clear();
                foreach (var receievedGrantItem in newData.ReceievedGrantItem)
                {
                    beforeUserData.ReceievedGrantItem.Add(receievedGrantItem);
                }

                beforeUserData.CompletedAchievements.Clear();
                foreach (var completedAchievement in newData.CompletedAchievements)
                {
                    beforeUserData.CompletedAchievements.Add(completedAchievement);
                }

                await context.SaveChangesAsync();
            }
        }

        public void PlayGameEvents(UserAccountDetail user, IEnumerable<GameEvent> gameEvents)
        {
            foreach (var gameEvent in gameEvents)
            {
                PlayGameEvent(user, gameEvent);
            }
        }

        public void PlayGameEvent(UserAccountDetail user, GameEvent gameEvent)
        {
            switch (gameEvent.EventType)
            {
                case nameof(CharacterGachaEvent):
                    PlayCharacterGachaEvent(user, JsonSerializer.Deserialize<CharacterGachaEvent>(gameEvent.Payload));
                    break;
                case nameof(CharacterRankUpEvent):
                    _characterService.PlayCharacterRankUpEvent(user, JsonSerializer.Deserialize<CharacterRankUpEvent>(gameEvent.Payload));
                    break;
                case nameof(GainAcheivementRewardsEvent):
                    _achievementService.PlayGainAcheivementRewardsEvent(user, JsonSerializer.Deserialize<GainAcheivementRewardsEvent>(gameEvent.Payload));
                    break;
                case nameof(GetMissionRewardEvent):
                    _requestMissionService.PlayGetMissionRewardEvent(user, JsonSerializer.Deserialize<GetMissionRewardEvent>(gameEvent.Payload));
                    break;
                case nameof(GrantItemToMailBoxEvent):
                    _userRepository.PlayGrantItemToMailBoxEvent(user, JsonSerializer.Deserialize<GrantItemToMailBoxEvent>(gameEvent.Payload));
                    break;
                case nameof(RequestMissionStartEvent):
                    _requestMissionService.PlayRequestMissionStartEvent(user, JsonSerializer.Deserialize<RequestMissionStartEvent>(gameEvent.Payload));
                    break;
                case nameof(UseLevelUpItemEvent):
                    _characterService.PlayUseLevelUpItemEvent(user, JsonSerializer.Deserialize<UseLevelUpItemEvent>(gameEvent.Payload));
                    break;
                case nameof(DeleteAllEvent):
                    PlayDeleteAllEvent(user, JsonSerializer.Deserialize<DeleteAllEvent>(gameEvent.Payload));
                    break;
                case nameof(ShowMetheMoneyEvent):
                    PlayShowMetheMoneyEvent(user, JsonSerializer.Deserialize<ShowMetheMoneyEvent>(gameEvent.Payload));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown event type: {gameEvent.EventType}");
            }
        }

        public void PlayCharacterGachaEvent(UserAccountDetail user, CharacterGachaEvent e)
        {
            user.Crystal().Count = e.AfterCrystal;
            user.Characters.Add(DefaultGameCharacter.Create(e.AddCharacterCode));
        }

        public void PlayDeleteAllEvent(UserAccountDetail user, DeleteAllEvent e)
        {
            user.Characters.Clear();
        }

        public void PlayShowMetheMoneyEvent(UserAccountDetail user, ShowMetheMoneyEvent e)
        {
            foreach (var modifiedItem in e.ModifiedItems)
            {
                user.GameItem(modifiedItem.ItemName).Count = modifiedItem.AfterCount;
            }
        }
    }
}
