using SampleWebApi.Model.DbContexts;

namespace SampleWebApi.Model.Events
{
    public class RequestMissionStartEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string MissionCode { get; set; }
        public List<string> CharacterCodes { get; set; } = new();
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId　= UserId,
                EventType = nameof(RequestMissionStartEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }

        public static RequestMissionStartEvent Create(int userId, string missionCode, List<string> chracterCodes)
        {
            return new()
            {
                UserId = userId,
                MissionCode = missionCode,
                CharacterCodes = chracterCodes
            };
        }
    }
}
