using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class UserAccountDetailCreatedEvent
    {
        public string Username { get; set; } = string.Empty;
        public int UserId { get; set; }

        public int ShardNumber { get; set; }
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(UserAccountDetailCreatedEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
