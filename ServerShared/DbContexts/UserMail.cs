using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerShared.DbContexts
{
    public class UserMail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public List<GameItem> Items { get; set; } = new();

        public int UserAccountDetailUserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserAccountDetailUserId))]
        public UserAccountDetail UserAccountDetail { get; set; }
    }
}
