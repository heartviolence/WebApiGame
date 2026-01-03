using SampleWebApi.Model.RequestMissions;

namespace SampleWebApi.Service
{
    public interface IRequestMissionProvider
    {
        public Dictionary<string, RequestMissionSheetRecord> Missions { get; }
    }
}
