using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class GrantItemToMailBoxEvent : IGameEvent
    {
        public int UserId { get; set; }

        public List<GrantItem> ReceievedItems { get; set; } = new();
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(GrantItemToMailBoxEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
