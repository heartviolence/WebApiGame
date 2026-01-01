using SampleWebApi.Model.DbContexts;

namespace SampleWebApi.Model.Events
{
    public class CharacterGachaEvent : IGameEvent
    {
        public int UserId { get; set; }

        public string AddCharacterCode { get; set; }

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = this.UserId,
                EventType = nameof(CharacterGachaEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
