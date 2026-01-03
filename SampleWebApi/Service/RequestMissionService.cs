using SampleWebApi.Model.Characters;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Model.Events;
using SampleWebApi.Model.Items;
using SampleWebApi.Model.RequestMissions;
using System.Reflection;

namespace SampleWebApi.Service
{
    public class RequestMissionService
    {
        ILogger _logger;
        IRequestMissionProvider _missionProvider;
        IGameCharacterDataProvider _gameCharacterDataProvider;
        public RequestMissionService(IRequestMissionProvider missionProvider, IGameCharacterDataProvider gameCharacterDataProvider, ILogger<RequestMissionService> logger)
        {
            this._missionProvider = missionProvider;
            this._gameCharacterDataProvider = gameCharacterDataProvider;
            this._logger = logger;
        }

        public bool IsValidCharacterCodes(List<string> characterCodes)
        {
            var originCount = characterCodes.Count;

            if (originCount == 0)
            {
                _logger.LogWarning("캐릭터코드 0개");
                return false;
            }

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

        public bool IsValidMissionCode(IEnumerable<string> dbMissionCodes, string missionCode)
        {
            if (!_missionProvider.Missions.TryGetValue(missionCode, out var mission))
            {
                _logger.LogWarning("올바르지않은 의뢰 미션 코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            if (dbMissionCodes.Contains(missionCode))
            {
                _logger.LogWarning("이미 존재하는 의뢰 미션코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            return true;
        }

        public bool IsMissionSuccess(string missionCode, List<GameCharacter> characters)
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

        public List<GetMissionRewardEvent> ProcessCompleteMission(UserInfo userData, string completedMissionCode)
        {
            List<GetMissionRewardEvent> events = new List<GetMissionRewardEvent>();
            var rewards = _missionProvider.Missions[completedMissionCode].Rewards;

            foreach (var reward in rewards)
            {
                switch (reward.ItemCode)
                {
                    case SpeicalItemCodes.Crystal:
                        int beforeCrystal = userData.Crystal;
                        userData.Crystal += reward.MinCount;
                        _logger.LogInformation("User의 크리스탈+{Crystal},적용후+{CrystalCurrent}", reward.MinCount, userData.Crystal);
                        events.Add(new GetMissionRewardEvent()
                        {
                            UserId = userData.Id,
                            ItemCode = reward.ItemCode,
                            BeforeItemCount = beforeCrystal,
                            AeforeItemCount = userData.Crystal
                        });
                        break;
                    default:
                        throw new Exception("등록되지않은 아이템 코드");
                }
            }

            return events;
        }

        public bool IsMissionComplete(RequestMission mission)
        {
            return mission.StartTime + TimeSpan.FromSeconds(5) <= DateTime.Now;
        }
    }
}
