using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public class UserAccountCreatedEvent : IGameEvent
    {
        public string Username { get; set; } = string.Empty;
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = -1,
                EventType = nameof(UserAccountCreatedEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
