using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class UseLevelUpItemEvent : IGameEvent
    {
        public int UserId { get; set; }
        public int CharacterLevel { get; set; }
        public string CharacterName { get; set; }
        public List<ModifiedItemCountInfo> ModifiedItemCountInfo { get; set; } = new();
        public int CharacterEXP { get; set; }

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(UseLevelUpItemEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}