using SampleWebApi.Model.Items;

namespace SampleWebApi.Model.RequestMissions
{
    public class MissionReward
    {
        public string ItemCode { get; set; }

        bool IsRange { get; set; } = false;// true라면 MinCount~MaxCount,false라면 MinCount
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }

    public static class MissionRewards
    {
        public static MissionReward Crystal(int count)
        {
            return new MissionReward
            {
                ItemCode = SpeicalItemNames.Crystal,
                MinCount = count,
            };
        }
    }
}
