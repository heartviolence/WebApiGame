using ServerShared.DbContexts;
using System.Text.Json;

namespace ServerShared.Events
{
    public class GetMissionRewardEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string CompletedMissionCode { get; set; }
        public List<ModifiedItemCountInfo> ModifiedItems { get; set; } = new();
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(GetMissionRewardEvent),
                Payload = JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version
            };
        }
    }
}
