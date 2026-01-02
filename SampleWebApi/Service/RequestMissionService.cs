using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Characters;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Model.Events;
using SampleWebApi.Model.Items;
using SampleWebApi.Model.RequestMissions;

namespace SampleWebApi.Service
{
    public class RequestMissionService
    {
        RequestMissionProvider _missionProvider;
        GameCharacterDataProvider _gameCharacterDataProvider;
        ILogger _logger;
        public RequestMissionService(RequestMissionProvider missionProvider, GameCharacterDataProvider gameCharacterDataProvider, ILogger<RequestMissionService> logger)
        {
            this._missionProvider = missionProvider;
            this._gameCharacterDataProvider = gameCharacterDataProvider;
            this._logger = logger;
        }

        public async Task<bool> StartMission(int userId, string missionCode, List<string> characterCodes)
        {
            if (!IsValidCharacterCodes(characterCodes))
            {
                return false;
            }

            using (var context = new GameDbContext())
            {
                var user = await context.UserInfos.Where(u => u.Id == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.RequestMissions)
                    .FirstOrDefaultAsync();

                if (!IsValidMissionCode(user.RequestMissions, missionCode))
                {
                    return false;
                }

                var characters = user.Characters.Where(c => characterCodes.Contains(c.CharacterID)).ToList();
                if (characters.Count != characterCodes.Count)
                {
                    _logger.LogWarning("요청 캐릭터 코드에 해당하는 캐릭터가 존재하지않음");
                    return false;
                }

                var missionSuccess = IsMissionSuccess(missionCode, characters);
                if (!missionSuccess)
                {
                    _logger.LogWarning("미션 조건에 부합하지않음");
                    return false;
                }

                user.RequestMissions.Add(RequestMission.Create(missionCode));
                context.GameEvents.Add(RequestMissionStartEvent.Create(userId, missionCode, characterCodes).CovertToGameEvent());

                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task RequestMissionCompleteCheck(int userId)
        {
            using (var context = new GameDbContext())
            {
                var user = await context.UserInfos.Where(u => u.Id == userId)
                    .Include(u => u.RequestMissions)
                    .FirstOrDefaultAsync();

                var completeMissions = user.RequestMissions.Where(m => m.StartTime + TimeSpan.FromSeconds(10) <= DateTime.Now);
                var missionsRewards = completeMissions.Select(m => _missionProvider.Missions[m.MissionCode].Rewards);
                foreach (var rewards in missionsRewards)
                {
                    foreach (var reward in rewards)
                    {
                        context.GameEvents.Add(ProcessGetRewards(user, reward).CovertToGameEvent());
                    }
                }
                context.RequestMissions.RemoveRange(completeMissions);
                await context.SaveChangesAsync();
            }
        }

        bool IsValidCharacterCodes(List<string> characterCodes)
        {
            var originCount = characterCodes.Count;
            if (originCount > 3)
            {
                _logger.LogWarning("캐릭터코드 3개초과");
                return false;
            }

            //중복 확인 
            if (originCount != characterCodes.Distinct().Count())
            {
                _logger.LogWarning("중복 캐릭터코드 존재");
                return false;
            }

            return true;
        }

        bool IsValidMissionCode(List<RequestMission> dbData, string missionCode)
        {
            if (!_missionProvider.Missions.TryGetValue(missionCode, out var mission))
            {
                _logger.LogWarning("올바르지않은 의뢰 미션 코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            if (dbData.Select(m => m.MissionCode).Contains(missionCode))
            {
                _logger.LogWarning("이미 존재하는 의뢰 미션코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            return true;
        }

        bool IsMissionSuccess(string missionCode, List<GameCharacter> characters)
        {
            if (!_missionProvider.Missions.TryGetValue(missionCode, out var mission))
            {
                _logger.LogWarning("올바르지않은 의뢰 미션 코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            if (characters.Count(c => c.Level < mission.MinRequiredLevel) > 0)
            {
                return false;
            }

            var gameCharacterData = _gameCharacterDataProvider.GameCharacterData;
            int[] typeCounts = new int[(int)GameCharacterType.Count];

            foreach (var character in characters)
            {
                var characterType = gameCharacterData[character.CharacterID].Type;
                typeCounts[(int)characterType] = typeCounts[(int)characterType] + 1;
            }

            foreach (var requiredType in mission.RequiredCharacterTypes)
            {
                typeCounts[(int)requiredType] = typeCounts[(int)requiredType] - 1;
                if (typeCounts[(int)requiredType] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        GetMissionRewardEvent ProcessGetRewards(UserInfo userData, MissionReward reward)
        {
            switch (reward.ItemCode)
            {
                case SpeicalItemCodes.Crystal:
                    int beforeCrystal = userData.Crystal;
                    userData.Crystal += reward.MinCount;
                    _logger.LogInformation("User의 크리스탈+{Crystal},적용후+{CrystalCurrent}", reward.MinCount, userData.Crystal);
                    return new GetMissionRewardEvent()
                    {
                        UserId = userData.Id,
                        ItemCode = reward.ItemCode,
                        BeforeItemCount = beforeCrystal,
                        AeforeItemCount = userData.Crystal
                    };
                default:
                    throw new Exception("등록되지않은 아이템 코드");
            }
        }
    }
}
