using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class CharacterRankUpEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string CharacterName { get; set; }
        public int BeforeRank { get; set; }
        public int AfterRank { get; set; }

        public List<ModifiedItemCountInfo> ModifiedItemCountInfo { get; set; } = new();

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(CharacterRankUpEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
