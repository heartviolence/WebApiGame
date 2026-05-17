using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class UserSnapshotEvent : IGameEvent
    {
        public UserAccountDetail UserData { get; set; }

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = this.UserData.UserId,
                EventType = nameof(UserSnapshotEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
