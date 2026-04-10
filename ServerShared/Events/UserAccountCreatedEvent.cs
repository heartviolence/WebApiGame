using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public class UserAccountCreatedEvent : IGameEvent
    {
        public string Username { get; set; } = string.Empty;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                EventType = nameof(UserAccountCreatedEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
