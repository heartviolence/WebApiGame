using SampleWebApi.Model.Characters;
using SampleWebApi.Service.Characters;
using ServerShared.DbContexts;
using ServerShared.Events;

namespace SampleWebApi.Service.RequestMissions
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

        public bool IsValidMissionCode(string missionCode)
        {
            if (!_missionProvider.Missions.TryGetValue(missionCode, out var mission))
            {
                _logger.LogWarning("올바르지않은 의뢰 미션 코드,missionCode:{MissionCode}", missionCode);
                return false;
            }

            return true;
        }

        public void PlayGetMissionRewardEvent(UserAccountDetail user, GetMissionRewardEvent e)
        {
            foreach (var modifiedItem in e.ModifiedItems)
            {
                var gameItem = user.GameItems.Where(i => i.Name == modifiedItem.ItemName).FirstOrDefault();
                if (gameItem != null)
                {
                    gameItem.Count = modifiedItem.AfterCount;
                }
                else
                {
                    user.GameItems.Add(new GameItem() 
                    {
                        Name = modifiedItem.ItemName,
                        Count = modifiedItem.AfterCount,
                    });
                }
            }

            user.RequestMissions.RemoveAll(m => m.MissionCode == e.CompletedMissionCode);
        }

        public void PlayRequestMissionStartEvent(UserAccountDetail user, RequestMissionStartEvent e)
        {
            var requestMission = new RequestMission()
            {
                MissionCode = e.MissionCode,
                StartTime = e.StartTime,
            };
            user.RequestMissions.Add(requestMission);
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
                var characterType = gameCharacterData[character.Name].Type;
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

        public GetMissionRewardEvent ProcessCompleteMission(UserAccountDetail userData, string completedMissionCode)
        {
            var rewards = _missionProvider.Missions[completedMissionCode].Rewards;

            var gameEvent = new GetMissionRewardEvent()
            {
                UserId = userData.UserId,
                CompletedMissionCode = completedMissionCode
            };

            foreach (var reward in rewards)
            {
                var gameItem = userData.GameItems.Where(i => i.Name == reward.ItemName).FirstOrDefault();
                var beforeCount = 0;
                if (gameItem != null)
                {
                    beforeCount = gameItem.Count;
                }
                gameEvent.ModifiedItems.Add(new ModifiedItemCountInfo()
                {
                    ItemName = reward.ItemName,
                    BeforeCount = beforeCount,
                    AfterCount = beforeCount + reward.MinCount
                });
            }

            PlayGetMissionRewardEvent(userData, gameEvent);
            return gameEvent;
        }

        public bool IsMissionComplete(RequestMission mission)
        {
            return mission.StartTime + TimeSpan.FromSeconds(5) <= DateTime.Now;
        }
    }
}
