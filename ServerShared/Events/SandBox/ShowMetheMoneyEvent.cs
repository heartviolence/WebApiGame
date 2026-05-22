using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events.SandBox
{
    public class ShowMetheMoneyEvent : IGameEvent
    {
        public int UserId { get; set; }
        public List<ModifiedItemCountInfo> ModifiedItems { get; set; } = new();
        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = this.UserId,
                EventType = nameof(ShowMetheMoneyEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
