using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class GainAcheivementRewardsEvent : IGameEvent
    {
        public int UserId { get; set; }
        public string AchievementName { get; set; }
        public int Level { get; set; }
        public int BeforeRewardCheckPoint { get; set; }
        public int AfterRewardCheckPoint { get; set; }

        public List<GameItem> Rewards { get; set; } = new();

        public GameEvent CovertToGameEvent()
        {
            return new GameEvent
            {
                UserId = UserId,
                EventType = nameof(GainAcheivementRewardsEvent),
                Payload = System.Text.Json.JsonSerializer.Serialize(this),
                EventVersion = ServerVersion.Version,
            };
        }
    }
}
