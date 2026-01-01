using SampleWebApi.Model.DbContexts;

namespace SampleWebApi.Model.Events
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
