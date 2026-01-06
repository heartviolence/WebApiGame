using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public class UserCreateEvent : IGameEvent
    {
        public string Username { get; set; } = string.Empty;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                EventType = nameof(UserCreateEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
