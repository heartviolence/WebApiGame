using SampleWebApi.Model.Characters;

namespace SampleWebApi.Model.RequestMissions
{
    public class RequestMissionSheetRecord
    {
        public string MissionCode { get; set; }

        public int MinRequiredLevel { get; set; }

        public List<GameCharacterType> RequiredCharacterTypes { get; set; } = new();

        public List<MissionReward> Rewards { get; set; } = new();
    }
}
