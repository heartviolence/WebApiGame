using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerShared.DbContexts
{
    public class RequestMission
    {
        public int Id { get; set; }
        public string MissionCode { get; set; }

        public DateTime StartTime { get; set; }

        public int UserAccountDetailUserId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserAccountDetailUserId))]
        public UserAccountDetail UserAccountDetail { get; set; }        
    }
}
