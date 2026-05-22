using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events.SandBox
{
    public class DeleteAllEvent : IGameEvent
    {
        public int UserId { get; set; }

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = this.UserId,
                EventType = nameof(DeleteAllEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
