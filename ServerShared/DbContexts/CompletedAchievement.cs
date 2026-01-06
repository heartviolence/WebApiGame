using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class CompletedAchievement
    {
        public int Id { get; set; }
        public string AchievementCode { get; set; }

        public int Level { get; set; }
    }
}
