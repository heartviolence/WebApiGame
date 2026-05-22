using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerShared.DbContexts
{
    public class CompletedAchievement
    {
        public int Id { get; set; }
        public string AchievementName { get; set; }

        public int Level { get; set; }

        public int RewardCheckPoint { get; set; } = 0;

        public int UserAccountDetailUserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserAccountDetailUserId))]
        public UserAccountDetail UserAccountDetail { get; set; }
    }
}
