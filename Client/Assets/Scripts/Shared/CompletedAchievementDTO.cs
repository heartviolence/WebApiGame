using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Shared
{
    public class CompletedAchievementDTO
    {
        public string AchievementCode { get; set; }

        public int Level { get; set; }

        public int RewardCheckPoint { get; set; } = 0;
    }
}
