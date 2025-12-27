using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SampleWebApi.Model.DbContexts
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
    }
}
