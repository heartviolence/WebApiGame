using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public class RequestMissionStartEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string MissionCode { get; set; }
        public DateTime StartTime { get; set; }
        public List<string> CharacterCodes { get; set; } = new();
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(RequestMissionStartEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
