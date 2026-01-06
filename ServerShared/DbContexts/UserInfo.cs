using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ServerShared.DbContexts
{

    [Index(nameof(Username), IsUnique = true)]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public string? Nickname { get; set; }

        public List<GameCharacter> Characters { get; set; } = new();

        public int Crystal { get; set; }

        public List<RequestMission> RequestMissions { get; set; } = new();
        public List<CompletedAchievement> CompletedAchievements { get; set; } = new();

        public AchievementsData AchievementData { get; set; } = new();
    }
}
