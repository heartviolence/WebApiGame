using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public class CharacterGachaEvent : IGameEvent
    {
        public int UserId { get; set; }

        public string AddCharacterCode { get; set; }

        public int BeforeCrystal { get; set; }

        public int AfterCrystal { get; set; }
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

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
