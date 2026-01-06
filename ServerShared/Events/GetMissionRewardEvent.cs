using ServerShared.DbContexts;
using System.Text.Json;

namespace ServerShared.Events
{
    public class GetMissionRewardEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string ItemCode { get; set; }
        public int BeforeItemCount { get; set; }
        public int AeforeItemCount { get; set; }
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
