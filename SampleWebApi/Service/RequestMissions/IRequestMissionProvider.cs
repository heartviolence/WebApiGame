using SampleWebApi.Model.RequestMissions;

namespace SampleWebApi.Service.RequestMissions
{
    public interface IRequestMissionProvider
    {
        public Dictionary<string, RequestMissionSheetRecord> Missions { get; }
    }
}
