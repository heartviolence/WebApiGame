using SampleWebApi.Model.Characters;
using SampleWebApi.Model.RequestMissions;

namespace SampleWebApi.Service
{
    public class RequestMissionProvider : IRequestMissionProvider
    {
        public Dictionary<string, RequestMissionSheetRecord> Missions { get; private set; }
        public RequestMissionProvider()
        {
            Initialize();
        }

        void Initialize()
        {
            Missions = new Dictionary<string, RequestMissionSheetRecord>();
            Missions.Add("00-00-01", new RequestMissionSheetRecord()
            {
                MissionCode = "00-00-01",
                MinRequiredLevel = 1,
                RequiredCharacterTypes = { GameCharacterType.Dealer, GameCharacterType.Dealer },
                Rewards = new() { MissionRewards.Crystal(100) }
            });
            Missions.Add("00-00-02", new RequestMissionSheetRecord()
            {
                MissionCode = "00-00-02",
                MinRequiredLevel = 1,
                RequiredCharacterTypes = { GameCharacterType.Dealer, GameCharacterType.Balance, GameCharacterType.Support }
            });
        }
    }
}
