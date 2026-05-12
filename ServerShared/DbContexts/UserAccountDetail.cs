using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ServerShared.DbContexts
{

    [Index(nameof(Username), IsUnique = true)]
    public class UserAccountDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        public string? Nickname { get; set; }

        public List<GameCharacter> Characters { get; set; } = new();

        public int Crystal { get; set; }

        public List<RequestMission> RequestMissions { get; set; } = new();
        public List<CompletedAchievement> CompletedAchievements { get; set; } = new();

        public AchievementsData AchievementData { get; set; } = new();

        public List<GameItem> GameItems { get; set; } = new();

        public List<RecordItem> Records { get; set; } = new();

        public string? GameState { get; set; }

        public List<UserMail> MailBox { get; set; } = new();
        public List<ReceievedGrantItem> ReceievedGrantItem { get; set; } = new();

        [ConcurrencyCheck]
        public Guid RowVersion { get; set; } //해당 테이블의 항목 또는 하위 항목을 업데이트 할때마다 값을 변경해야함
    }
}
